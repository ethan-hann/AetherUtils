using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security.Encryption;

namespace AetherUtils.Tests;

public class GlobalTests
{
    private readonly string _passphrase = "super secure password!";
    private readonly string _licenseId = Guid.NewGuid().ToString();
    private License _license;
    private readonly string _filePath = "files\\License.lic";
    
    [SetUp]
    public void SetUp()
    {
        _license = new License()
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