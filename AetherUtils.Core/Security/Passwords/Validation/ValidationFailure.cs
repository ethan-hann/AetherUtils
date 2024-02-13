namespace AetherUtils.Core.Security.Passwords.Validation;

/// <summary>
/// Represents a validation failure when checking a password against a password rule.
/// </summary>
/// <param name="message">The message associated with the failure.</param>
/// <param name="howToResolve">Indicates how to fix the failure.</param>
public class ValidationFailure(string message, string howToResolve) : IPasswordValidationFailure
{
    public string Message { get; set; } = message;
    public string HowToResolve { get; set; } = howToResolve;
    
    public ValidationFailure() : this(string.Empty, string.Empty) {}
}