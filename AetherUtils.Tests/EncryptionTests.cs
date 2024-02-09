using System.Text;
using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security.Encryption;

namespace AetherUtils.Tests
{
    public class EncryptionTests
    {
        private string testString = "Somewhere over the rainbow!!!";
        private readonly string testFilePath = @"%temp%\AetherTests\files\TestEncryptedFileSave.enc";
        private Guid testLicenseID = Guid.NewGuid();
        const string passphrase = "secure_password!";


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
        public void TestMultipleStringEncryption()
        {
            var service = new StringEncryptionService();

            for (int i = 0; i < 10; i++)
            {
                var encrypted = service.EncryptAsync(testString, passphrase).Result;
                Console.WriteLine($"Encrypted Data: {Convert.ToBase64String(encrypted)}");
            }
        }

        [Test]
        public void TestRoundTripObject()
        {
            var service = new ObjectEncryptionService<License>();

            License l = new License();
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
            License l = new License();
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
            string passKey = EncryptionBase.GetRandomKeyPhrase();

            Console.WriteLine($"Passphrase: {passKey}");

            var encrypted = service.EncryptAsync(testString, passKey);
            Console.WriteLine($"Encrypted: {encrypted.Result}");

            var decrypted = service.DecryptAsync(encrypted.Result, passKey);
            Console.WriteLine($"Decrypted: {decrypted.Result}");

            Assert.That(decrypted.Result, Is.EqualTo(testString));
        }

        [Test]
        public async Task TestExistingFileEncrypt()
        {
            var service = new FileEncryptionService(testFilePath);
            
            Console.WriteLine($"Passphrase: {passphrase}");

            var newFilePath = await FileEncryptionService.EncryptFileAsync("files\\fast internet.png", passphrase);
            Console.WriteLine($"Encrypted: {newFilePath}");
        }

        [Test]
        public async Task TestExistingFileDecrypt()
        {
            var service = new FileEncryptionService(testFilePath);
            Console.WriteLine($"Passphrase: {passphrase}");

            var newFilePath = await FileEncryptionService.DecryptFileAsync("files\\fast internet.enc", passphrase);
            Console.WriteLine($"Decrypted: {newFilePath}");
        }

        [Test]
        public void GetCharacterCode()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(new[] { '█' });
            Console.WriteLine(bytes.ToPrintableString());
        }
        //
        // [Test]
        // public void TestExistingFileDecrypt()
        // {
        //     var service = new FileEncryptionService(testFilePath);
        //     service.FilePath = "files\\Hello.txt";
        //
        //     var decrypted = service.DecryptAsync(service.FilePath, passphrase);
        //     Console.WriteLine($"Decrypted: {decrypted.Result}");
        // }
    }
}
