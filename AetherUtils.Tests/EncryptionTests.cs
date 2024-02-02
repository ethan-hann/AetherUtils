using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security;

namespace AetherUtils.Tests
{
    public class EncryptionTests
    {
        private byte[] Key = EncryptionService.GetRandomKey();
        private EncryptionService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new EncryptionService();
        }

        [Test]
        public void TestRoundTripString()
        {
            string testString = "Somewhere over the rainbow!";

            //Encrypt
            string encrypted = _service.EncryptString(testString, Key);
            Console.WriteLine("Encrypted: " + encrypted);

            //Verify the initial string and the encrypted string are not the same.
            Assert.That(encrypted, Is.Not.EqualTo(testString));

            //Decrypt
            string decrypted = _service.DecryptString(encrypted, Key);
            Console.WriteLine("Decrypted: " + decrypted);

            //Verify that the decrypted string is equal to our initial input string.
            Assert.That(decrypted, Is.EqualTo(testString));
        }

        [Test]
        public void TestRoundTripObject()
        {
            License l = new License();

            //Encrypt
            string? encrypted = _service.EncryptObject(l, Key);

            //Verify that the encrypted string is not null (indicating a failed encryption
            Assert.That(encrypted, Is.Not.Null);
            Console.WriteLine("Encrypted: " + encrypted);

            //Decrypt
            License? decrypted = _service.DecryptObject<License>(encrypted, Key);
            Assert.That(decrypted, Is.Not.Null);

            //Verify that the decrypted object is equal to the initial object.
            Assert.That(decrypted.Id, Is.EqualTo(l.Id));
        }
    }
}
