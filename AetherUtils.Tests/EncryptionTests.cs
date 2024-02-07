using AetherUtils.Core.Files;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security;
using AetherUtils.Core.Security.Encryption;
using System.Text;

namespace AetherUtils.Tests
{
    public class EncryptionTests
    {
        private readonly byte[] Key = EncryptionService2.GetRandomKey();
        private string testString = "Somewhere over the rainbow!!!";
        private string testFilePath = "files\\TestEncryptedFileSave.enc";
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

        //[Test]
        //public void TestRoundTripString()
        //{
        //    //Encrypt string
        //    byte[] encrypted = EncryptionService.EncryptString(testString, Key);
        //    Console.WriteLine("Encrypted: " + encrypted);

        //    //Verify the initial string and the encrypted string are not the same.
        //    Assert.That(encrypted, Is.Not.EqualTo(testString));

        //    //Decrypt
        //    var decrypted = EncryptionService.DecryptString(encrypted, Key);
        //    Console.WriteLine("Decrypted: " + decrypted.Result);

        //    //Verify that the decrypted string is equal to our initial input string.
        //    Assert.That(decrypted.Result, Is.EqualTo(testString));
        //}

        //[Test]
        //public void TestRoundTripObject()
        //{
        //    License l = new License()
        //    {
        //        Id = testLicenseID.ToString()
        //    };

        //    //Encrypt
        //    byte[]? encrypted = EncryptionService.EncryptObject(l, Key);

        //    //Verify that the encrypted string is not null (indicating a failed encryption
        //    Assert.That(encrypted, Is.Not.Null);
        //    Console.WriteLine("Encrypted: " + encrypted);

        //    //Decrypt
        //    License? decrypted = EncryptionService.DecryptObject<License>(encrypted, Key);
        //    Assert.That(decrypted, Is.Not.Null);

        //    Console.WriteLine("License ID: " + decrypted.Id);

        //    //Verify that the decrypted object is equal to the initial object.
        //    Assert.That(decrypted.Id, Is.EqualTo(l.Id));
        //}

        //[Test]
        //public void TestEncryptedStringFileSave()
        //{
        //    bool isEncrypted = EncryptionService.EncryptFile("Hello World!", testFilePath, Key);
        //    Assert.That(isEncrypted, Is.True);

        //}

        //[Test]
        //public void TestEncryptedStringFileLoad()
        //{
        //    var decrypted = EncryptionService.DecryptFile(testFilePath, Key);
        //    Console.WriteLine(decrypted);

        //    Assert.That(decrypted, Is.EqualTo("Hello World!"));
        //}

        //[Test]
        //public void TestEncryptedFileSave()
        //{
        //    License l = new License()
        //    {
        //        Id = testLicenseID.ToString()
        //    };

        //    //Encrypt
        //    bool isEncrypted = EncryptionService.EncryptObjectToFile(l, testFilePath, Key);
        //    Assert.That(isEncrypted, Is.True);

        //    //Assert the file was created.
        //    Assert.That(FileHelper.DoesFileExist(testFilePath));

        //    //byte[]? encrypted = EncryptionService.EncryptObject(l, Key);

        //    //Verify that the encrypted string is not null (indicating a failed encryption
        //    //Assert.That(encrypted, Is.Not.Null);
        //    //Console.WriteLine("Encrypted: " + encrypted);

        //    //Save to file
        //    // EncryptionService.EncryptFile(Encoding.UTF8.GetString(encrypted), testFilePath, Key);
        //}

        //[Test]
        //public void TestEncryptedFileLoad()
        //{
        //    ////Load from file
        //    //var encryptedContents = EncryptionService.DecryptFile("files\\TestEncryptedFileSave.enc", Key);

        //    //Console.WriteLine("Encrypted: " + encryptedContents.Result);

        //    //Decrypt
        //    License? decrypted = EncryptionService.DecryptObjectFromFile<License>(testFilePath, Key);

        //    Assert.That(decrypted, Is.Not.Null);

        //    Console.WriteLine("License ID: " + decrypted.Id);

        //    //Verify that the decrypted object is equal to the initial object.
        //    Assert.That(decrypted.Id, Is.EqualTo(testLicenseID.ToString()));
        //}
    }
}
