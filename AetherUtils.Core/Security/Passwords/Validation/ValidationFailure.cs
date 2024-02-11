namespace AetherUtils.Core.Security.Passwords.Validation;

public class ValidationFailure : IPasswordValidationFailure
{
    public string? Message { get; set; }
    public string? HowToResolve { get; set; }
}