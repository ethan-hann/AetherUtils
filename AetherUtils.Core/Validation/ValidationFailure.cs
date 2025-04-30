// // ValidationFailure.cs : AetherUtils
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

namespace AetherUtils.Core.Validation;

/// <summary>
///     Represents a validation failure when checking a password against a password rule.
/// </summary>
/// <param name="message">The message associated with the failure.</param>
/// <param name="howToResolve">Indicates how to fix the failure.</param>
public sealed class ValidationFailure(string message, string howToResolve) : IValidationFailure
{
    /// <summary>
    ///     Create a new validation failure.
    /// </summary>
    public ValidationFailure() : this(string.Empty, string.Empty) { }

    /// <summary>
    ///     Gets or sets the message that describes the password validation failure.
    /// </summary>
    public string Message { get; set; } = message;

    /// <summary>
    ///     Gets or sets the message that describes how to resolve the validation failure.
    /// </summary>
    public string HowToResolve { get; set; } = howToResolve;
}