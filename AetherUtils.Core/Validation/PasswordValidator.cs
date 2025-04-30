// PasswordValidator.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using AetherUtils.Core.Security.Passwords;

namespace AetherUtils.Core.Validation;

/// <summary>
///     Validates a password against a <see cref="PasswordRule" />.
///     <remarks>
///         This class cannot be inherited and should only be
///         called from an instance of <see cref="PasswordRule" /> to validate a password.
///     </remarks>
/// </summary>
public abstract class PasswordValidator
{
    //List borrowed from: https://owasp.org/www-community/password-special-characters
    //Excluding a space character since that is handled in the WhiteSpaceChars array.
    internal static readonly char[] SpecialChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".ToCharArray();
    internal static readonly char[] RegularChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    internal static readonly char[] NumberChars = "1234567890".ToCharArray();
    internal static readonly char[] WhiteSpaceChars = " ".ToCharArray();

    private PasswordValidator() { }

    /// <summary>
    ///     Validate the <paramref name="password" /> against the specified <see cref="PasswordRule" />.
    /// </summary>
    /// <param name="rule">The <see cref="PasswordRule" /> used for validation.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>
    ///     A list of <see cref="IValidationFailure" />.
    ///     If validation was successful, this list is empty.
    /// </returns>
    internal static List<IValidationFailure> Validate(PasswordRule rule, string password)
    {
        var data = rule.RuleData;
        List<IValidationFailure> failures = [];

        if (!data.SpecialsAllowed && password.Any(c => SpecialChars.Contains(c)))
            failures.Add(new ValidationFailure
            {
                Message = "Password is not allowed to contain special characters.",
                HowToResolve = "Remove the special characters from the password and validate again."
            });

        if (!data.WhitespaceAllowed && password.Any(c => WhiteSpaceChars.Contains(c)))
            failures.Add(new ValidationFailure
            {
                Message = "Password is not allowed to contain any whitespace characters.",
                HowToResolve = "Remove the whitespace characters (space) from the password and validate again."
            });

        if (!data.NumbersAllowed && password.Any(c => NumberChars.Contains(c)))
            failures.Add(new ValidationFailure
            {
                Message = "Password is not allowed to contain any number characters.",
                HowToResolve = "Remove the numbers from the password and validate again."
            });

        if (data.MinimumLengthAllowed > -1 && password.Length < data.MinimumLengthAllowed)
            failures.Add(new ValidationFailure
            {
                Message = $"Password required length is {data.MinimumLengthAllowed}. The password was too short.",
                HowToResolve = "Add more characters to the password to increase its length."
            });

        var specialCount = password.Count(c => SpecialChars.Contains(c));
        var numberCount = password.Count(c => NumberChars.Contains(c));

        if (data.MinimumSpecialCount > -1 && specialCount < data.MinimumSpecialCount)
            failures.Add(new ValidationFailure
            {
                Message = "Password contained too few special characters.",
                HowToResolve = "Add more special characters to the password to increase complexity."
            });

        if (data.MinimumNumberCount > -1 && numberCount < data.MinimumNumberCount)
            failures.Add(new ValidationFailure
            {
                Message = "Password contained too few numbers.",
                HowToResolve = "Add more numbers to the password to increase complexity."
            });

        if (data.Expiration != null && DateTime.Now > data.Expiration)
            failures.Add(new ValidationFailure
            {
                Message = "Password has expired!",
                HowToResolve = "Set a new password and update the expiration date."
            });


        return failures;
    }
}