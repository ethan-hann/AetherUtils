// TwoFactorUser.cs : AetherUtils
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

namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
///     Represents a 2-factor authentication user.
/// </summary>
/// <param name="issuer">The issuer of the 2FA tokens.</param>
/// <param name="accountTitle">The account title as shown in the user's authenticator app.</param>
public sealed class TwoFactorUser(string issuer, string accountTitle)
{
    /// <summary>
    ///     The issuer of the 2FA tokens.
    /// </summary>
    public string Issuer { get; } = issuer;

    /// <summary>
    ///     The account title as shown in the user's authenticator app.
    /// </summary>
    public string AccountTitle { get; internal set; } = accountTitle;

    /// <summary>
    ///     The information needed for the user to set the account up in their authenticator app.
    /// </summary>
    public SetupCode? SetupInformation { get; internal set; }
}