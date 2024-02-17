using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using AetherUtils.Core.Enums;
using AetherUtils.Core.Exceptions;
using AetherUtils.Core.Files;
using AetherUtils.Core.Security.Encryption;
using AetherUtils.Core.Security.Passwords.Validation;
using AetherUtils.Core.Utility;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Represents a password rule with various options. A password rule should be built using the
/// Rule Builder factory: <see cref="IPasswordRuleBuilder"/>.
/// <para>This class contains methods to parse and save a password rule to/from a file as well as a
/// string.</para>
/// <remarks>A password can be validated against a rule using <see cref="PasswordRule.Validate"/>.</remarks>
/// </summary>
public sealed class PasswordRule
{
    private bool _isBuilding;
    private bool _isBuilt;

    internal PasswordRuleData RuleData = new();

    private static readonly Json<PasswordRuleData> Serializer = new();
    private static readonly StringEncryptionService Encryptor = new();

    /// <summary>
    /// Get the string representing this password rule.
    /// </summary>
    public string PasswordRules { get; private set; } = string.Empty;
    
    internal PasswordRule(bool isBuilt)
    {
        _isBuilt = isBuilt;
        _isBuilding = !_isBuilt;
    }

    private PasswordRule(string json)
    {
        var p = Serializer.FromJson(json);
        if (p == null) return;
        
        RuleData = p;
        CompileRules();
        
        _isBuilt = true;
        _isBuilding = false;
    }
    
    /// <summary>
    /// Start building the password rule.
    /// </summary>
    /// <returns>The builder instance.</returns>
    public static IPasswordRuleBuilder New() => new PasswordRuleBuilder();

    /// <summary>
    /// Allows a password to contain whitespace.
    /// </summary>
    /// <exception cref="PasswordRuleException">If the password rule is not being built.</exception>
    internal void AllowWhiteSpace()
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        RuleData.WhitespaceAllowed = true;
    }

    /// <summary>
    /// Allows a password to contain special characters.
    /// </summary>
    /// <exception cref="PasswordRuleException">If the password rule is not being built.</exception>
    internal void AllowSpecials()
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        RuleData.SpecialsAllowed = true;
    }

    /// <summary>
    /// Allows a password to contain numbers.
    /// </summary>
    /// <exception cref="PasswordRuleException">If the password rule is not being built.</exception>
    internal void AllowNumbers()
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        RuleData.NumbersAllowed = true;
    }

    /// <summary>
    /// Set the minimum length a password should be.
    /// </summary>
    /// <param name="length">The length the password should be.</param>
    /// <exception cref="PasswordRuleException">If the password rule is not being built or
    /// <paramref name="length"/> is less than 0.</exception>
    internal void MinimumLength(int length)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rule is not being built!");
        if (length < 0)
            throw new PasswordRuleException("Password minimum length cannot be less than 0.");
        
        RuleData.MinimumLengthAllowed = length;
    }

    /// <summary>
    /// Set the minimum count of numbers a password should contain.
    /// </summary>
    /// <param name="count">The minimum count of numbers the password should contain.</param>
    /// <exception cref="PasswordRuleException">If the password rule is not being built or
    /// <paramref name="count"/> is less than 0.</exception>
    internal void MinimumNumberCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rule is not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum number count cannot be less than 0.");

        RuleData.MinimumNumberCount = count;
    }

    /// <summary>
    /// Set the minimum count of special characters a password should contain.
    /// </summary>
    /// <param name="count">The minimum count of special characters the password should contain.</param>
    /// <exception cref="PasswordRuleException">If the password rule is not being built or
    /// <paramref name="count"/> is less than 0.</exception>
    internal void MinimumSpecialCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum letter count cannot be less than 0.");

        RuleData.MinimumSpecialCount = count;
    }

    /// <summary>
    /// Set the date that passwords should expire at.
    /// </summary>
    /// <param name="expires">The expiration date of passwords validated against the rule.</param>
    /// <exception cref="PasswordRuleException">If the password is not being built or
    /// <paramref name="expires"/> is outside of the range: <see cref="DateTime.MinValue"/> to <see cref="DateTime.MaxValue"/>.</exception>
    internal void Expires(DateTime expires)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (expires < DateTime.MinValue || expires > DateTime.MaxValue)
            throw new PasswordRuleException("Password expiration was outside of valid range.");

        RuleData.Expiration = expires;
    }

    /// <summary>
    /// Finish building the password rule and compile.
    /// After this method is called, no more parameters can be added to the password rule.
    /// </summary>
    internal void FinishedBuilding()
    {
        CompileRules();

        _isBuilding = PasswordRules.Equals(string.Empty);
        _isBuilt = !_isBuilding;
    }

    private void CompileRules() => PasswordRules = ToJsonString();
    
    private string ToJsonString() => Serializer.ToJson(RuleData);
    
    /// <summary>
    /// Parse a password rule from a Json string.
    /// </summary>
    /// <param name="json">The password rule as Json to parse.</param>
    /// <returns>The <see cref="PasswordRule"/> or <c>null</c> if parsing failed.</returns>
    public static PasswordRule Parse(string json) => new(json);

    /// <summary>
    /// Get an encrypted string representing this password rule.
    /// </summary>
    /// <param name="passphrase">The passphrase used for encryption.</param>
    /// <returns>A base64 string representing the password rule.</returns>
    public string ToEncryptedString(string passphrase)
    {
        var json = ToJsonString();
        var encrypted = Encryptor.EncryptAsync(json, passphrase);
        return Convert.ToBase64String(encrypted.Result);
    }

    /// <summary>
    /// Parse a password rule from an encrypted string.
    /// </summary>
    /// <param name="encrypted">The encrypted password rule to parse.</param>
    /// <param name="passphrase">The passphrase used for decryption.</param>
    /// <returns>The <see cref="PasswordRule"/> or <c>null</c> if parsing failed.</returns>
    public static PasswordRule FromEncryptedString(string encrypted, string passphrase)
    {
        var bytes = Convert.FromBase64String(encrypted);
        var decrypted = Encryptor.DecryptAsync(bytes, passphrase);
        var rule = Parse(decrypted.Result);
        return rule;
    }
    
    /// <summary>
    /// Save the encrypted password rule to a file.
    /// </summary>
    /// <param name="filePath">The path to the file to save; can contain Windows path variables (i.e., <c>%temp%</c>)</param>
    /// <param name="passphrase">The passphrase used for encryption.</param>
    /// <returns><c>true</c> if the password rule saved successfully; <c>false</c> otherwise.</returns>
    public async Task<bool> SaveToFileAsync(string filePath, string passphrase)
    {
        var json = ToJsonString();
        var bytes = await Encryptor.EncryptAsync(json, passphrase);
        FileHelper.SaveFileAsync(filePath, bytes);

        return FileHelper.DoesFileExist(filePath);
    }

    /// <summary>
    /// Parse an encrypted password rule from a file.
    /// </summary>
    /// <param name="filePath">The path to the file to load; can contain Windows path variables (i.e., <c>%temp%</c>)</param>
    /// <param name="passphrase">The passphrase used for decryption. Should be the same as the one used for encryption.</param>
    /// <returns>The <see cref="PasswordRule"/> or <c>null</c> if parsing failed.</returns>
    public static async Task<PasswordRule?> ParseFromFileAsync(string filePath, string passphrase)
    {
        var encryptedBytes = await FileHelper.OpenNonTextFileAsync(filePath);
        var json = await Encryptor.DecryptAsync(encryptedBytes, passphrase);
        var data = Serializer.FromJson(json);
        
        if (data == null) return null;

        var rule = new PasswordRule(true)
        {
            RuleData = data
        };
        
        rule.CompileRules();
        return rule;
    }

    /// <summary>
    /// Get a random, cryptographically strong password that follows the password rule.
    /// </summary>
    /// <returns>A new, random password that adheres to the password rule created.</returns>
    public string GetValidPassword()
    {
        var sb = new StringBuilder();
        
        //Keep track of the current count of characters in the new password string.
        //Should always end equal to RuleData.MinimumLengthAllowed.
        var currentCount = 0;
        
        //Define the different types of characters allowed in the password with their likelihoods of being chosen.
        //Characters are 60% likely to be chosen.
        //Specials are 20% likely to be chosen.
        //Numbers are 15% likely to be chosen.
        //Whitespace is 5% likely to be chosen.
        ProportionalRandomSelector<CharacterType> selector = GetSelector(60, 20, 
            15, 5);
        
        //Set the minimum length of our new password to the length defined in the rule data.
        var minLength = RuleData.MinimumLengthAllowed;
        
        //Keep track of the number of different character types in the new password string.
        var specialCount = 0;
        var numberCount = 0;
        
        //Continue looping until we've reached the required minimum password length.
        while (currentCount != minLength)
        {
            //First, get a random character type
            var c = selector.SelectItem();

            //Then, switch on the type and add appropriate character to the string, increasing counts along the way.
            switch (c)
            {
                case CharacterType.WhiteSpace:
                    AddWhiteSpace(ref sb, ref currentCount);
                    break;
                case CharacterType.Special:
                    AddSpecial(ref sb, ref currentCount, ref specialCount);
                    break;
                case CharacterType.Number:
                    AddNumber(ref sb, ref currentCount, ref numberCount);
                    break;
                case CharacterType.Character:
                    AddCharacter(ref sb, ref currentCount, ref numberCount, ref specialCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.WriteLine($"{c}: {currentCount}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Get a <see cref="ProportionalRandomSelector{T}"/> based on the supplied percentages.
    /// Each percentage indicates the likelihood for that character type to be chosen.
    /// </summary>
    /// <returns>A <see cref="ProportionalRandomSelector{T}"/> that can be used to select items
    /// based on percentages.</returns>
    private ProportionalRandomSelector<CharacterType> GetSelector(int characterPercentage, int specialPercentage,
        int numberPercentage, int whiteSpacePercentage)
    {
        var characterTypes = Enum.GetValues(typeof(CharacterType)).Cast<CharacterType>().ToList();
        var disallowedTypes = new List<CharacterType>();
        
        //remove the character types not allowed
        if (!RuleData.WhitespaceAllowed)
            disallowedTypes.Add(CharacterType.WhiteSpace);
        if (!RuleData.NumbersAllowed)
            disallowedTypes.Add(CharacterType.Number);
        if (!RuleData.SpecialsAllowed)
            disallowedTypes.Add(CharacterType.Special);

        characterTypes = characterTypes.Except(disallowedTypes).ToList();

        ProportionalRandomSelector<CharacterType> selector = new();
        
        //We can't just add all of the percentages since we don't know ahead of time which types of characters are
        //allowed in the password. So, we have to loop through our derived list and only add the percentages that
        //are allowed.
        foreach (var type in characterTypes)
        {
            switch (type)
            {
                case CharacterType.Character:
                    selector.AddPercentage(type, characterPercentage);
                    break;
                case CharacterType.Special:
                    selector.AddPercentage(type, specialPercentage);
                    break;
                case CharacterType.Number:
                    selector.AddPercentage(type, numberPercentage);
                    break;
                case CharacterType.WhiteSpace:
                    selector.AddPercentage(type, whiteSpacePercentage);
                    break;
            }
        }

        return selector;
    }

    /// <summary>
    /// Add a whitespace character to the random password, if allowed.
    /// </summary>
    /// <param name="builder">The current <see cref="StringBuilder"/> reference.</param>
    /// <param name="currentCount">The current loop count reference.</param>
    private void AddWhiteSpace(ref StringBuilder builder, ref int currentCount)
    {
        //if whitespace is not allowed in the rule
        if (!RuleData.WhitespaceAllowed) return;
        
        //Finally add a whitespace character if these fail
        builder.Append(PasswordValidator.WhiteSpaceChars[RandomNumberGenerator.GetInt32(
                                                         PasswordValidator.WhiteSpaceChars.Length)]);
        currentCount++;
    }

    /// <summary>
    /// Add a special character to the random password, if allowed.
    /// </summary>
    /// <param name="builder">The current <see cref="StringBuilder"/> reference.</param>
    /// <param name="currentCount">The current loop count reference.</param>
    /// <param name="specialCount">The current count of specials reference.</param>
    /// <returns><c>true</c> if the character was added; <c>false</c> otherwise.</returns>
    private bool AddSpecial(ref StringBuilder builder, ref int currentCount, ref int specialCount)
    {
        //if special characters are not allowed in the rule
        if (!RuleData.SpecialsAllowed) return false;
        
        //If no minimum specials are defined, we'll use the default of 2.
        var numOfSpecialsAllowed = RuleData.MinimumSpecialCount <= -1 ? 2 : RuleData.MinimumSpecialCount;

        //If we've already reached the number of specials required, just return.
        if (specialCount == numOfSpecialsAllowed) return false;
        
        //Otherwise, lets add a special character and increment our counts
        builder.Append(PasswordValidator.SpecialChars[RandomNumberGenerator.GetInt32(
                                                      PasswordValidator.SpecialChars.Length)]);
        currentCount++;
        specialCount++;
        return true;
    }

    /// <summary>
    /// Add a number to the random password, if allowed.
    /// </summary>
    /// <param name="builder">The current <see cref="StringBuilder"/> reference.</param>
    /// <param name="currentCount">The current loop count reference.</param>
    /// <param name="numberCount">The current count of numbers reference.</param>
    /// <returns><c>true</c> if the number was added; <c>false</c> otherwise.</returns>
    private bool AddNumber(ref StringBuilder builder, ref int currentCount, ref int numberCount)
    {
        //If numbers are not allowed in the rule
        if (!RuleData.NumbersAllowed) return false;
        
        //If no minimum number count is defined, we'll use the default of 2.
        var numOfNumbersAllowed = RuleData.MinimumNumberCount <= -1 ? 2 : RuleData.MinimumNumberCount;
        
        //If we've already reached the number of digits required, just return.
        if (numberCount == numOfNumbersAllowed) return false;
        
        //Otherwise, let's add a number character and increment our counts.
        builder.Append(PasswordValidator.NumberChars[RandomNumberGenerator.GetInt32(
                                                     PasswordValidator.NumberChars.Length)]);
        currentCount++;
        numberCount++;
        return true;
    }

    /// <summary>
    /// Add an alphabet character to the random password.
    /// </summary>
    /// <param name="builder">The current <see cref="StringBuilder"/> reference.</param>
    /// <param name="currentCount">The current loop count reference.</param>
    /// <param name="numberCount">The current count of numbers reference.</param>
    /// <param name="specialCount">The current count of specials reference.</param>
    private void AddCharacter(ref StringBuilder builder, ref int currentCount, ref int numberCount, ref int specialCount)
    {
        //Try to add a number first.
        if (AddNumber(ref builder, ref currentCount, ref numberCount)) return;

        //Finally, add a character if we've met the required number of both specials and digits.
        builder.Append(PasswordValidator.RegularChars[RandomNumberGenerator.GetInt32(
            PasswordValidator.RegularChars.Length)]);
        
        //Then try to add a special character.
        if (AddSpecial(ref builder, ref currentCount, ref specialCount)) return;
        
        currentCount++;
    }

    /// <summary>
    /// Validate a password against this rule.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>A list of <see cref="IPasswordValidationFailure"/> containing the reasons the
    /// password failed to validate. If this list is empty, the password validated successfully.</returns>
    public List<IPasswordValidationFailure> Validate(string password) => PasswordValidator.Validate(this, password);

    /// <summary>
    /// Get the Json string representing this password rule.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToJsonString();
}