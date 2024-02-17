using Newtonsoft.Json;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Holds the data relating to a <see cref="PasswordRule"/> built using <see cref="IPasswordRuleBuilder"/>.
/// </summary>
public sealed class PasswordRuleData
{
    public bool WhitespaceAllowed { get; set; }
    public bool SpecialsAllowed { get; set; }
    public bool NumbersAllowed { get; set; }
    public int MinimumLengthAllowed { get; set; } = 12;
    public int MinimumNumberCount { get; set; } = -1;
    public int MinimumSpecialCount { get; set; } = -1;
    public DateTime? Expiration { get; set; }
    
    internal PasswordRuleData() { }
}