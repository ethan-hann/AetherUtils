// ConvertTests.cs : AetherUtils
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
using AetherUtils.Core.Extensions;
using AetherUtils.Core.Security.Hashing;

namespace AetherUtils.Tests;

public class ConvertTests
{
    private readonly string base64Encoded = "SGVsbG8h";
    private readonly string hexEncoded = "48656C6C6F21";
    private readonly string strToEncode = "Hello!";

    [SetUp]
    public void SetUp() { }

    [Test]
    public void ConvertToBase64Test()
    {
        var base64 = strToEncode.StringToEncodedString(HashEncoding.Base64);
        Console.WriteLine(base64);

        Assert.That(base64, Is.Not.Empty);
        Assert.That(base64, Is.Not.EqualTo(strToEncode));
        Assert.That(base64, Is.EqualTo(base64Encoded));
    }

    [Test]
    public void ConvertToHexTest()
    {
        var hex = strToEncode.StringToEncodedString(HashEncoding.Hex);
        Console.WriteLine(hex);

        Assert.That(hex, Is.Not.Empty);
        Assert.That(hex, Is.Not.EqualTo(strToEncode));
        Assert.That(hex, Is.EqualTo(hexEncoded));
    }

    [Test]
    public void ConvertBytesToBase64Test()
    {
        var base64 = strToEncode.StringToEncodedString(HashEncoding.Base64);
        var bytes = base64.BytesFromString();

        var converted = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(converted);

        Assert.That(converted, Is.Not.Empty);
        Assert.That(converted, Is.Not.EqualTo(strToEncode));
        Assert.That(converted, Is.EqualTo(base64Encoded));
    }

    [Test]
    public void ConvertBytesToHexTest()
    {
        var hex = strToEncode.StringToEncodedString(HashEncoding.Hex);
        var bytes = hex.BytesFromString();

        var converted = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(converted);

        Assert.That(converted, Is.Not.Empty);
        Assert.That(converted, Is.Not.EqualTo(strToEncode));
        Assert.That(converted, Is.EqualTo(hexEncoded));
    }
}