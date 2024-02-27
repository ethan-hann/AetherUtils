using Newtonsoft.Json;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Holds the data relating to a <see cref="PasswordRule"/> built using <see cref="IPasswordRuleBuilder"/>.
/// </summary>
public sealed class PasswordRuleData
{
    //List taken from: https://owasp.org/www-community/password-special-characters
    //Excluding a space character since that is handled in the WhiteSpaceChars array.
    
    /// <summary>
    /// Get or set the special characters allowed in a password.
    /// </summary>
    internal char[] SpecialChars { get; set; } = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".ToCharArray();
    
    /// <summary>
    /// Get or set the regular English alphabet characters allowed in a password.
    /// </summary>
    internal char[] RegularChars { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    /// Get or set the integers allowed in a password.
    /// </summary>
    internal char[] NumberChars { get; set; } = ['1','2','3','4','5','6','7','8','9','0'];
    
    /// <summary>
    /// Get or set the white space characters allowed in a password.
    /// </summary>
    internal char[] WhiteSpaceChars { get; set; } = " ".ToCharArray();

    /// <summary>
    /// Get or set the prefix that should be appended to the beginning of the password.
    /// </summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Get or set the suffix that should be appended to the end of the password.
    /// </summary>
    public string Suffix { get; set; } = string.Empty;

    /// <summary>
    /// Get or set the template text that was used to build this rule.
    /// </summary>
    /// <remarks>This string is only populated if the rule was built using <see cref="PasswordRule.BuildFromTemplate"/> otherwise
    /// it is an empty string.</remarks>
    public string TemplateText { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether to append the current calender year to the end of a password.
    /// </summary>
    public bool AppendCurrentYear { get; set; } = false;
    
    /// <summary>
    /// Indicates that whitespace is allowed in a password.
    /// </summary>
    public bool WhitespaceAllowed { get; set; }
    
    /// <summary>
    /// Indicates that special characters are allowed in a password.
    /// </summary>
    public bool SpecialsAllowed { get; set; }
    
    /// <summary>
    /// Indicates that numbers are allowed in a password.
    /// </summary>
    public bool NumbersAllowed { get; set; }
    
    /// <summary>
    /// The minimum length a password is allowed to be.
    /// </summary>
    public int MinimumLengthAllowed { get; set; } = 12;
    
    /// <summary>
    /// The minimum count of number characters a password should contain.
    /// </summary>
    public int MinimumNumberCount { get; set; } = -1;
    
    /// <summary>
    /// The minimum count of special characters a password should contain.
    /// </summary>
    public int MinimumSpecialCount { get; set; } = -1;
    
    /// <summary>
    /// The expiration date that a password expires at.
    /// </summary>
    public DateTime? Expiration { get; set; }
    
    internal PasswordRuleData() { }
}