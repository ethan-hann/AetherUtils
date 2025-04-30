// TwoFactorTests.cs : AetherUtils
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

using System.Text;
using AetherUtils.Core.Security.TwoFactor;

namespace AetherUtils.Tests;

public class TwoFactorTests
{
    [Test]
    public void SetupTwoFactorTest()
    {
        var secret = "12345678901234567890123456789012";
        var secretAsByteArray = Encoding.UTF8.GetBytes(secret);
        var issuer = "Test";
        var accountName = "TestAccount";
        var expected = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZA";

        var u = new TwoFactorUser(issuer, accountName);
        var tfa = new TwoFactorAuth();

        tfa.GenerateSetupInformation(ref u, secretAsByteArray);
        Assert.That(u.SetupInformation, Is.Not.Null);
        Assert.That(u.SetupInformation.ManualEntryKey, Is.EqualTo(expected));
    }

    [Test]
    public void SetupWithQrCodeTest()
    {
        var secret = "12345678901234567890123456789012";
        var secretAsByteArray = Encoding.UTF8.GetBytes(secret);
        var issuer = "Test";
        var accountName = "TestAccount";
        var expected = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZA";

        var u = new TwoFactorUser(issuer, accountName);
        var tfa = new TwoFactorAuth();

        tfa.GenerateSetupInformation(ref u, secretAsByteArray);
        Assert.That(u.SetupInformation, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(u.SetupInformation.ManualEntryKey, Is.EqualTo(expected));
            Assert.That(u.SetupInformation.QrCodeSetupImageUrl, Is.Not.Empty);
        });
        Console.WriteLine(u.SetupInformation.QrCodeSetupImageUrl);
    }
}