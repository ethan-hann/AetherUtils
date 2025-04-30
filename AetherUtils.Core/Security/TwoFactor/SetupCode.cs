// SetupCode.cs : AetherUtils
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
///     Represents a setup code needed by the user to set up two-factor authentication.
/// </summary>
/// <param name="manualEntryKey">The secret key that can be manually entered in a 2FA app.</param>
/// <param name="qrCodeSetupImageUrl">The base64 string representing the QR code.</param>
public sealed class SetupCode(string manualEntryKey, string qrCodeSetupImageUrl)
{
    /// <summary>
    ///     The secret key that can be manually entered in a 2FA app.
    /// </summary>
    public string ManualEntryKey { get; } = manualEntryKey;

    /// <summary>
    ///     The base64 string representing the QR code.
    /// </summary>
    public string QrCodeSetupImageUrl { get; } = qrCodeSetupImageUrl;
}