using AetherUtils.Core.Licensing;
using AetherUtils.Core.Structs;
using AetherUtils.Core.Validation;
using Standard.Licensing;
using Standard.Licensing.Security.Cryptography;

namespace AetherUtils.Tests;

public class ValidationTests
{
    private const string emailAddress = "anyone@outlook.com";
    private const string invalidEmail = "anyone.com";
    private const string invalidEmail2 = "joe@.com";
    private const string invalidEmail3 = "";
    private const string invalidEmail4 = "joe123@do.";
    private const string passphrase = "somewhere over the rainbow";
    
    [Test]
    public void EmailValidationTest()
    {
        List<IValidationFailure> failures = new();
        
        failures = EmailValidator.Validate(emailAddress);
        failures.ForEach(f => Console.WriteLine(f.Message));
        Assert.That(failures, Is.Empty);

        failures = EmailValidator.Validate(invalidEmail);
        failures.ForEach(f => Console.WriteLine(f.Message));
        Assert.That(failures, Has.Count.EqualTo(1));
        
        failures = EmailValidator.Validate(invalidEmail2);
        failures.ForEach(f => Console.WriteLine(f.Message));
        Assert.That(failures, Has.Count.EqualTo(1));
        
        failures = EmailValidator.Validate(invalidEmail3);
        failures.ForEach(f => Console.WriteLine(f.Message));
        Assert.That(failures, Has.Count.EqualTo(2));
        
        failures = EmailValidator.Validate(invalidEmail4);
        failures.ForEach(f => Console.WriteLine(f.Message));
        Assert.That(failures, Has.Count.EqualTo(1));
    }

    [Test]
    public void ValidateLicenseTest()
    {
        var keyGenerator = KeyGenerator.Create();
        var keyPair = keyGenerator.GenerateKeyPair();
        var privateKey = keyPair.ToEncryptedPrivateKeyString(passphrase);
        var publicKey = keyPair.ToPublicKeyString();
            
        License l = License.New().LicensedTo("Test user", "test@test.com")
            .WithUniqueIdentifier(new Guid()).WithMaximumUtilization(5)
            .CreateAndSignWithPrivateKey(privateKey, passphrase);
        Pair<string, string> licensePair = new(l.ToString(), publicKey);
        var failures = LicenseValidator.Validate([licensePair]);
        Assert.That(failures, Is.Empty);
    }
}