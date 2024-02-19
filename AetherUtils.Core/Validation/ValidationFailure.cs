namespace AetherUtils.Core.Validation;

/// <summary>
/// Represents a validation failure when checking a password against a password rule.
/// </summary>
/// <param name="message">The message associated with the failure.</param>
/// <param name="howToResolve">Indicates how to fix the failure.</param>
public sealed class ValidationFailure(string message, string howToResolve) : IValidationFailure
{
    /// <summary>
    /// Gets or sets the message that describes the password validation failure.
    /// </summary>
    public string Message { get; set; } = message;
    
    /// <summary>
    /// Gets or sets the message that describes how to resolve the validation failure.
    /// </summary>
    public string HowToResolve { get; set; } = howToResolve;
    
    /// <summary>
    /// Create a new validation failure.
    /// </summary>
    public ValidationFailure() : this(string.Empty, string.Empty) {}
}