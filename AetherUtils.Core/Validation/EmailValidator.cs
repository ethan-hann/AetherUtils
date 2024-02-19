using AetherUtils.Core.RegEx;

namespace AetherUtils.Core.Validation;

/// <summary>
/// Provides a <see cref="Validate"/> method to validate an email address.
/// </summary>
public abstract class EmailValidator : IValidator<string>
{
    /// <summary>
    /// Validate email addresses. This validator checks against a Regular expression.
    /// Also, it checks for an empty string.
    /// </summary>
    /// <param name="emails">The email addresses to validate.</param>
    /// <returns>A list of <see cref="IValidationFailure"/> indicating the issues.
    /// If validation passed for all supplied emails, this list is empty.</returns>
    public static List<IValidationFailure> Validate(params string[] emails)
    {
        List<IValidationFailure> failures = [];
        foreach (var email in emails)
        {
            if (email.Length <= 0)
                failures.Add(new ValidationFailure($"{email} -> Email address length was 0.", 
                    "Resolve by adding characters to the email address"));
            if (!RegexGenerator.EmailRegex().IsMatch(email))
                failures.Add(new ValidationFailure($"{email} -> Email address was not a valid format.", 
                    "Ensure the email address is a real address."));
        }
        
        return failures;
    }
}