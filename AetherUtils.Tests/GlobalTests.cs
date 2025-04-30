// GlobalTests.cs : AetherUtils
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

public class GlobalTests
{
    private readonly string _filePath = "files\\License.lic";
    private readonly string _licenseId = Guid.NewGuid().ToString();
    private readonly string _passphrase = "super secure password!";
    private License _license;

    [SetUp]
    public void SetUp()
    {
        _license = new License
        {
            Id = _licenseId
        };
    }

    [Test]
    public async Task TestCreateLicenseSaveAndEncrypt()
    {
        //var licenseString = _license.Serialize();
        var encryptor = new ObjectEncryptionService<License>();

        var bytes = await encryptor.EncryptToFileAsync(_license, _filePath, _passphrase);

        Assert.That(bytes, Is.Not.Empty);
    }

    [Test]
    public async Task TestLoadLicenseAndDecrypt()
    {
        var decryptor = new ObjectEncryptionService<License>();
        var license = await decryptor.DecryptFromFileAsync(_filePath, _passphrase);
        Console.WriteLine(license.ToString());
        //Assert.That(license.Id, Is.EqualTo(_licenseId));
    }
}