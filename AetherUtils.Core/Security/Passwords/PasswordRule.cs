using System.Text;
using AetherUtils.Core.Exceptions;
using AetherUtils.Core.Extensions;
using AetherUtils.Core.Files;
using AetherUtils.Core.Security.Encryption;
using AetherUtils.Core.Security.Hashing;
using AetherUtils.Core.Security.Passwords.Validation;

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
    /// Set the minimum number of letters a password should contain.
    /// </summary>
    /// <param name="count">The minimum letter count the password should contain.</param>
    /// <exception cref="PasswordRuleException">If the password rule is not being built or
    /// <paramref name="count"/> is less than 0.</exception>
    internal void MinimumLetterCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rule is not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum letter count cannot be less than 0.");
        
        RuleData.MinimumLetterCount = count;
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

        if (!PasswordRules.Equals(string.Empty))
        {
            _isBuilding = false;
            _isBuilt = true;
        }
        else
        {
            _isBuilding = true;
            _isBuilt = false;
        }
    }

    private void CompileRules() => PasswordRules = ToJsonString();
    
    private string ToJsonString() => Serializer.ToJson(RuleData);
    
    /// <summary>
    /// Parse a password rule from a Json string.
    /// </summary>
    /// <param name="json">The password rule as Json to parse.</param>
    /// <returns>The <see cref="PasswordRule"/> or <c>null</c> if parsing failed.</returns>
    public static PasswordRule? Parse(string json) => new(json);

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
    public static PasswordRule? FromEncryptedString(string encrypted, string passphrase)
    {
        var bytes = Convert.FromBase64String(encrypted);
        var decrypted = Encryptor.DecryptAsync(bytes, passphrase);
        var rule = Parse(decrypted.Result);
        return rule ?? null;
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
    /// <exception cref="NotImplementedException"></exception>
    public string GetValidPassword()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Validate a password against the password rule.
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