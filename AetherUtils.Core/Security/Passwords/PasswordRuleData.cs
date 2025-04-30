// // PasswordRuleData.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
///     Holds the data relating to a <see cref="PasswordRule" /> built using <see cref="IPasswordRuleBuilder" />.
/// </summary>
public sealed class PasswordRuleData
{
    internal PasswordRuleData() { }
    /// <summary>
    ///     Indicates that whitespace is allowed in a password.
    /// </summary>
    public bool WhitespaceAllowed { get; set; }

    /// <summary>
    ///     Indicates that special characters are allowed in a password.
    /// </summary>
    public bool SpecialsAllowed { get; set; }

    /// <summary>
    ///     Indicates that numbers are allowed in a password.
    /// </summary>
    public bool NumbersAllowed { get; set; }

    /// <summary>
    ///     The minimum length a password is allowed to be.
    /// </summary>
    public int MinimumLengthAllowed { get; set; } = 12;

    /// <summary>
    ///     The minimum count of number characters a password should contain.
    /// </summary>
    public int MinimumNumberCount { get; set; } = -1;

    /// <summary>
    ///     The minimum count of special characters a password should contain.
    /// </summary>
    public int MinimumSpecialCount { get; set; } = -1;

    /// <summary>
    ///     The expiration date that a password expires at.
    /// </summary>
    public DateTime? Expiration { get; set; }
}