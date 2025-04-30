// EmailValidator.cs : AetherUtils
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

using AetherUtils.Core.RegEx;

namespace AetherUtils.Core.Validation;

/// <summary>
///     Provides a <see cref="Validate" /> method to validate an email address.
/// </summary>
public abstract class EmailValidator : IValidator<string>
{
    /// <summary>
    ///     Validate email addresses. This validator checks against a Regular expression.
    ///     Also, it checks for an empty string.
    /// </summary>
    /// <param name="emails">The email addresses to validate.</param>
    /// <returns>
    ///     A list of <see cref="IValidationFailure" /> indicating the issues.
    ///     If validation passed for all supplied emails, this list is empty.
    /// </returns>
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