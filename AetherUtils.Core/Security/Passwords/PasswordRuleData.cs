using Newtonsoft.Json;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Holds the data relating to a <see cref="PasswordRule"/> built using <see cref="IPasswordRuleBuilder"/>.
/// </summary>
public sealed class PasswordRuleData
{
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