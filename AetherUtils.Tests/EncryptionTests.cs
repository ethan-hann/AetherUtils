﻿using System.Text;
using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security.Encryption;

namespace AetherUtils.Tests
{
    public class EncryptionTests
    {
        private string testString = "Somewhere over the rainbow!!!";
        private readonly string testFilePath = "files\\TestEncryptedFileSave.enc";
        private readonly string testDecryptedFile = "files\\testPair.keys";
        private readonly string testEncryptedFile = "files\\testPairEnc.keys";
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
        public void GetByteCount()
        {
            Console.WriteLine("AU\x01"u8.ToArray().Length);
        }

        [Test]
        public async Task IsEncryptedFileTest()
        {
            //await FileEncryptionService.EncryptFileAsync(testEncryptedFile, passphrase, ".keys");
            await FileEncryptionService.DecryptFileAsync(testEncryptedFile, passphrase);
            
            Assert.That(FileEncryptionService.IsEncryptedFile(testEncryptedFile), Is.False);
            Assert.That(FileEncryptionService.IsEncryptedFile(testDecryptedFile), Is.False);
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
}
