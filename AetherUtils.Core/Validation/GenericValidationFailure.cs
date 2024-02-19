namespace AetherUtils.Core.Validation;

/// <summary>
/// Represents a generic validation failure.
/// </summary>
public class GenericValidationFailure : IValidationFailure
{
    /// <summary>
    /// Gets or sets the message that describes the password validation failure.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message that describes how to resolve the validation failure.
    /// </summary>
    public string HowToResolve { get; set; } = string.Empty;
}