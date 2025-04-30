// IPasswordRuleBuilder.cs : AetherUtils
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

using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
///     Builder interface for creating a new <see cref="PasswordRule" />.
/// </summary>
public interface IPasswordRuleBuilder : IFluentInterface
{
    /// <summary>
    ///     Allows a password to contain whitespace.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowWhitespace();

    /// <summary>
    ///     Allows a password to contain special characters.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowSpecials();

    /// <summary>
    ///     Allows a password to contain numbers.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowNumbers();

    /// <summary>
    ///     Set the minimum length a password should be.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumLength(int length);

    /// <summary>
    ///     Set the minimum count of numbers a password should contain.
    /// </summary>
    /// <param name="count">The minimum number of digits allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumNumberCount(int count);

    /// <summary>
    ///     Set the minimum count of special characters a password should contain.
    /// </summary>
    /// <param name="count">The minimum number of special characters allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumSpecialCount(int count);

    /// <summary>
    ///     Set the date that a password should expire at.
    /// </summary>
    /// <param name="expires">The expiration expires of passwords validated against the rule.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder Expires(DateTime expires);

    /// <summary>
    ///     Build and compile the password rule.
    /// </summary>
    /// <returns>The built <see cref="PasswordRule" />.</returns>
    PasswordRule Build();
}