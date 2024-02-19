using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Validation;

/// <summary>
/// Represents a failure of a validation check.
/// </summary>
public interface IValidationFailure : IFluentInterface
{
    /// <summary>
    /// Gets or sets the message that describes the validation failure.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Gets or sets the message that describes how to resolve the validation failure.
    /// </summary>
    string HowToResolve { get; set; }
}