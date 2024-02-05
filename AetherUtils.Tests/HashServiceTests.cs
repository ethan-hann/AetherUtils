using AetherUtils.Core.Security;
using AetherUtils.Core.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Tests
{
    public class HashServiceTests
    {
        public HashService service;
        public HashOptions options;
        public string testString = "Somewhere over the rainbow!";
        public string testString2 = "Somwhere ver the rainbow";
        public string hashedString = string.Empty;

        [SetUp]
        public void SetUp()
        {
            options = new HashOptions(new ReadOnlyPair<int, int>(1000, 10000), HashEncoding.Hex);
            service = new HashService(options);
            hashedString = service.HashString(testString);
        }

        [Test]
        public void CreateHexHashTest()
        {
            options = new HashOptions(new ReadOnlyPair<int, int>(1000, 10000), HashEncoding.Hex);
            service = new HashService(options);
            hashedString = service.HashString(testString);

            Console.WriteLine("Original: " + testString);
            Console.WriteLine("Hashed (Hex): " + service.HashedString);

            //Assert that hashed string is not empty
            Assert.That(hashedString, Is.Not.Empty);

            //Assert that the string returned by the HashString function is equal to the internal hashed string of the object.
            Assert.That(hashedString, Is.EqualTo(service.HashedString));

            //Assert that the test string is equalvalent to the hashed string.
            Assert.That(service.CompareHash(testString), Is.True);

            //Assert that the testString2 is NOT equalvilent to the hashed string.
            Assert.That(HashService.CompareHash(testString2, hashedString), Is.False);
        }

        [Test]
        public void CreateBase64HashTest()
        {
            options = new HashOptions(new ReadOnlyPair<int, int>(1000, 10000), HashEncoding.Base64);
            service = new HashService(options);
            hashedString = service.HashString(testString);

            Console.WriteLine("Original: " + testString);
            Console.WriteLine("Hashed (Base64): " + service.HashedString);

            //Assert that hashed string is not empty
            Assert.That(hashedString, Is.Not.Empty);

            //Assert that the string returned by the HashString function is equal to the internal hashed string of the object.
            Assert.That(hashedString, Is.EqualTo(service.HashedString));

            //Assert that the test string is equalvalent to the hashed string.
            Assert.That(service.CompareHash(testString), Is.True);

            //Assert that the testString2 is NOT equalvilent to the hashed string.
            Assert.That(HashService.CompareHash(testString2, hashedString), Is.False);
        }

        [Test]
        public void CompareHashTest()
        {
            options = new HashOptions(new ReadOnlyPair<int, int>(1000, 10000), HashEncoding.Base64);
            service = new HashService(options);
            hashedString = service.HashString(testString);

            Console.WriteLine("Original: " + testString);
            Console.WriteLine("Non-Matching: " + testString2);
            Console.WriteLine("Hashed: " + hashedString);

            //Assert that the testString2 is NOT equalvilent to the hashed string.
            Assert.That(HashService.CompareHash(testString2, hashedString), Is.False);

            //Assert that the testString is equalvalent to the hashed string.
            Assert.That(HashService.CompareHash(testString, hashedString), Is.True);
        }

        [Test]
        public void CompareHashManualTest()
        {
            hashedString = "Hex:9998AFEC42D8A9467C7CE23736C17DD626994D81886F7D84BE310731E563BF4961E39686045B7BFAC6DF596107D1A8A7:20C0DDBD5F7FC62D66503ED9427CADC4:2827:SHA384";
            Assert.That(HashService.CompareHash(testString, hashedString), Is.True);
        }
    }
}
