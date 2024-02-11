﻿namespace AetherUtils.Core.Security.Passwords.Validation;

public abstract class PasswordValidator
{
    private static readonly char[] SpecialChars = "!?#$%^&*()-+=".ToCharArray();
    private static readonly char[] NumberChars = "1234567890".ToCharArray();
    private static readonly char[] WhiteSpaceChars = " ".ToCharArray();
    
    private PasswordValidator() {}

    internal static List<IPasswordValidationFailure> Validate(PasswordRule rule, string password)
    {
        var data = rule.RuleData;
        List<IPasswordValidationFailure> failures = [];
        
        if (!data.SpecialsAllowed && password.Any(c => SpecialChars.Contains(c)))
            failures.Add(new ValidationFailure()
            {
                Message = "Password is not allowed to contain special characters.",
                HowToResolve = "Remove the special characters from the password and validate again."
            });
        
        if (!data.WhitespaceAllowed && password.Any(c => WhiteSpaceChars.Contains(c)))
            failures.Add(new ValidationFailure()
            {
                Message = "Password is not allowed to contain any whitespace characters.",
                HowToResolve = "Remove the whitespace characters (space) from the password and validate again."
            });
        
        if (!data.NumbersAllowed && password.Any(c => NumberChars.Contains(c)))
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

        var specialCount = password.Count(c => SpecialChars.Contains(c));
        var numberCount = password.Count(c => NumberChars.Contains(c));
        
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
}