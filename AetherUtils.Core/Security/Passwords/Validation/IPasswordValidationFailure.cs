using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Security.Passwords.Validation;

/// <summary>
/// Represents a failure of a password rule check.
/// </summary>
public interface IPasswordValidationFailure : IFluentInterface
{
    /// <summary>
    /// Gets or sets the message that describes the password validation failure.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Gets or sets the message that describes how to resolve the validation failure.
    /// </summary>
    string HowToResolve { get; set; }
}