// EncryptionTests.cs : AetherUtils
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

using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security.Encryption;

namespace AetherUtils.Tests;

public class EncryptionTests
{
    private const string passphrase = "secure_password!";
    private readonly string testDecryptedFile = "files\\testPair.keys";
    private readonly string testEncryptedFile = "files\\testPairEnc.keys";
    private readonly string testFilePath = "files\\TestEncryptedFileSave.enc";
    private readonly Guid testLicenseID = Guid.NewGuid();
    private readonly string testString = "Somewhere over the rainbow!!!";


    [Test]
    public void TestRoundTripString()
    {
        var service = new StringEncryptionService();

        var encrypted = service.EncryptAsync(testString, passphrase);
        Console.WriteLine($"Encrypted Data: {Convert.ToBase64String(encrypted.Result)}");

        var decrypted = service.DecryptAsync(encrypted.Result, passphrase);
        Console.WriteLine($"Decrypted Data: {decrypted.Result}");
        Assert.That(decrypted.Result, Is.EqualTo(testString));
    }

    [Test]
    public void GetByteCount()
    {
        Console.WriteLine("AU\x01"u8.ToArray().Length);
    }

    [Test]
    public void TestMultipleStringEncryption()
    {
        var service = new StringEncryptionService();

        for (var i = 0; i < 10; i++)
        {
            var encrypted = service.EncryptAsync(testString, passphrase).Result;
            Console.WriteLine($"Encrypted Data: {Convert.ToBase64String(encrypted)}");
        }
    }

    [Test]
    public void TestRoundTripObject()
    {
        var service = new ObjectEncryptionService<License>();

        var l = new License();
        l.Id = testLicenseID.ToString();
        var encrypted = service.EncryptAsync(l, passphrase);
        Console.WriteLine($"Encrypted Data: {Convert.ToBase64String(encrypted.Result)}");

        var decrypted = service.DecryptAsync(encrypted.Result, passphrase);
        Console.WriteLine($"Decrypted Data: {decrypted.Result}");
        Assert.That(decrypted.Result.Id, Is.EqualTo(testLicenseID.ToString()));
    }

    [Test]
    public void TestRoundTripFile()
    {
        var service = new FileEncryptionService(testFilePath);
        var result = service.EncryptAsync(testString, passphrase);

        Console.WriteLine(result.Result);

        var decrypted = service.DecryptAsync(testFilePath, passphrase);
        Console.WriteLine($"Decrypted: {decrypted.Result}");

        Assert.That(decrypted.Result, Is.EqualTo(testString));
    }

    [Test]
    public void TestRoundTripObjectFile()
    {
        var service = new ObjectEncryptionService<License>();
        var l = new License();
        l.Id = testLicenseID.ToString();
        var encrypted = service.EncryptToFileAsync(l, testFilePath, passphrase);

        Console.WriteLine($"Encrypted Data: {encrypted.Result}");
        Assert.That(encrypted.Result, Is.Not.Empty);

        var decrypted = service.DecryptFromFileAsync(testFilePath, passphrase);
        Console.WriteLine($"Decrypted: {decrypted.Result}");

        Assert.That(decrypted.Result.Id, Is.EqualTo(testLicenseID.ToString()));
    }

    [Test]
    public void TestRandomPassPhrase()
    {
        var service = new StringEncryptionService();
        var passKey = EncryptionBase.GetRandomKeyPhrase();

        Console.WriteLine($"Passphrase: {passKey}");

        var encrypted = service.EncryptAsync(testString, passKey);
        Console.WriteLine($"Encrypted: {encrypted.Result}");

        var decrypted = service.DecryptAsync(encrypted.Result, passKey);
        Console.WriteLine($"Decrypted: {decrypted.Result}");

        Assert.That(decrypted.Result, Is.EqualTo(testString));
    }

    // [Test]
    // public void TestExistingFileEncrypt()
    // {
    //     var service = new FileEncryptionService(testFilePath);
    //     
    //     Console.WriteLine($"Passphrase: {passphrase}");
    //
    //     var newFilePath = FileEncryptionService.EncryptFileAsync("files\\fast internet.png", passphrase, 
    //         ".png");
    //     Console.WriteLine($"Encrypted: {newFilePath.Result}");
    // }
    //
    // [Test]
    // public void TestExistingFileDecrypt()
    // {
    //     var service = new FileEncryptionService(testFilePath);
    //     Console.WriteLine($"Passphrase: {passphrase}");
    //
    //     var newFilePath = FileEncryptionService.DecryptFileAsync("files\\fast internet.png", passphrase);
    //     Console.WriteLine($"Decrypted: {newFilePath.Result}");
    // }

    // [Test]
    // public void TestExistingFileEncrypt2()
    // {
    //     Console.WriteLine($"Passphrase: {passphrase}");
    //
    //     var newFilePath = FileEncryptionService.EncryptFileAsync("files\\Amount Owed to Papa for Gas.docx", passphrase, 
    //         ".docx");
    //     Console.WriteLine($"Encrypted: {newFilePath.Result}");
    // }
    //
    // [Test]
    // public void TestExistingFileDecrypt2()
    // {
    //     Console.WriteLine($"Passphrase: {passphrase}");
    //
    //     var newFilePath = FileEncryptionService.DecryptFileAsync("files\\Amount Owed to Papa for Gas.docx", passphrase);
    //     Console.WriteLine($"Decrypted: {newFilePath.Result}");
    // }
}