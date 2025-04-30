// ExtensionTests.cs : AetherUtils
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

using AetherUtils.Core.Extensions;
using AetherUtils.Core.Security.Encryption;
using AetherUtils.Core.Utility;

namespace AetherUtils.Tests;

public class ExtensionTests
{
    public SerializableDictionary<int, string> dictionary;

    [SetUp]
    public void SetUp()
    {
        //Create dictionary and populate
        dictionary = [];
        dictionary.Add(1, "First item");
        dictionary.Add(2, "Second item");
        dictionary.Add(3, "Third item");
    }

    [Test]
    public void RenameKeyTest()
    {
        dictionary.RenameKey(1, 10);

        //After rename, key (10) should be equal to the first item in the dictionary.
        Assert.That(dictionary[10], Is.EqualTo("First item"));
    }

    [Test]
    public void RenameKeyTest100()
    {
        for (var i = 1; i <= 100; i++)
        {
            dictionary.RenameKey(1, 10); //Rename key to 10.

            //After rename, key (10) should be equal to the first item in the dictionary.
            Assert.That(dictionary[10], Is.EqualTo("First item"));

            dictionary.RenameKey(10, 1); //Rename key back to 1.
        }
    }

    [Test]
    public void RenameKeyTestException()
    {
        SerializableDictionary<int, string> dictionary2 = [];

        dictionary2.Add(1, "First item");
        dictionary2.Add(2, "Second item");
        dictionary2.Add(3, "Third item");

        //Assert that an exception is thrown if we try to rename a key with a name that already exists in the dictionary.
        Assert.Throws<ArgumentException>(() => dictionary2.RenameKey(1, 2));
    }

    [Test]
    public void PrintableStringTest()
    {
        var original = EncryptionBase.GetRandomKey();
        var s = original.ToPrintableString();
        Console.WriteLine(s);

        var fromBytes = (byte[])s.FromPrintableString();

        Assert.That(original, Is.EqualTo(fromBytes));
    }
}