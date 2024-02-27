using System.Text;
using AetherUtils.Core.Security.Passwords;

namespace AetherUtils.Core.Validation;

/// <summary>
/// Validates a password against a <see cref="PasswordRule"/>.
/// <remarks>This class cannot be inherited and should only be
/// called from an instance of <see cref="PasswordRule"/> to validate a password.</remarks>
/// </summary>
public abstract class PasswordValidator
{
    private PasswordValidator() {}

    /// <summary>
    /// Validate the <paramref name="password"/> against the specified <see cref="PasswordRule"/>.
    /// </summary>
    /// <param name="rule">The <see cref="PasswordRule"/> used for validation.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>A list of <see cref="IValidationFailure"/>.
    /// If validation was successful, this list is empty.</returns>
    internal static List<IValidationFailure> Validate(PasswordRule rule, string password)
    {
        var data = rule.ruleData;
        
        //If we are using a template on this rule, validate against it instead.
        if (!data.TemplateText.Equals(string.Empty))
            return ValidateFromTemplate(rule, password);
        
        List<IValidationFailure> failures = [];
        
        if (!data.SpecialsAllowed && password.Any(c => data.SpecialChars.Contains(c)))
            failures.Add(new ValidationFailure()
            {
                Message = "Password is not allowed to contain special characters.",
                HowToResolve = "Remove the special characters from the password and validate again."
            });
        
        if (!data.WhitespaceAllowed && password.Any(c => data.WhiteSpaceChars.Contains(c)))
            failures.Add(new ValidationFailure()
            {
                Message = "Password is not allowed to contain any whitespace characters.",
                HowToResolve = "Remove the whitespace characters (space) from the password and validate again."
            });
        
        if (!data.NumbersAllowed && password.Any(c => data.NumberChars.Contains(c)))
            failures.Add(new ValidationFailure()
            {
                Message = "Password is not allowed to contain any number characters.",
                HowToResolve = "Remove the numbers from the password and validate again."
            });
        
        if (data.MinimumLengthAllowed > -1 && password.Length < data.MinimumLengthAllowed)
            failures.Add(new ValidationFailure()
            {
                Message = $"Password required length is {data.MinimumLengthAllowed}. The password was too short.",
                HowToResolve = "Add more characters to the password to increase its length."
            });

        var specialCount = password.Count(c => data.SpecialChars.Contains(c));
        var numberCount = password.Count(c => data.NumberChars.Contains(c));
        
        if (data.MinimumSpecialCount > -1 && specialCount < data.MinimumSpecialCount)
            failures.Add(new ValidationFailure()
            {
                Message = "Password contained too few special characters.",
                HowToResolve = "Add more special characters to the password to increase complexity."
            });
        
        if (data.MinimumNumberCount > -1 && numberCount < data.MinimumNumberCount)
            failures.Add(new ValidationFailure()
            {
                Message = "Password contained too few numbers.",
                HowToResolve = "Add more numbers to the password to increase complexity."
            });
        
        if (data.Expiration != null && DateTime.Now > data.Expiration)
            failures.Add(new ValidationFailure()
            {
                Message = "Password has expired!",
                HowToResolve = "Set a new password and update the expiration date."
            });
        
        
        return failures;
    }

    private static List<IValidationFailure> ValidateFromTemplate(PasswordRule rule, string password)
    {
        return [];
    }
}