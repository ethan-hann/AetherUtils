using AetherUtils.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Tests
{
    public class ConvertTests
    {
        string strToEncode = "Hello!";
        string base64Encoded = "SGVsbG8h";
        string hexEncoded = "48656C6C6F21";

        [SetUp]
        public void SetUp() { }

        [Test]
        public void ConvertToBase64Test()
        {
            string base64 = strToEncode.StringToEncodedString(Core.Security.HashEncoding.Base64);
            Console.WriteLine(base64);

            Assert.That(base64, Is.Not.Empty);
            Assert.That(base64, Is.Not.EqualTo(strToEncode));
            Assert.That(base64, Is.EqualTo(base64Encoded));
        }

        [Test]
        public void ConvertToHexTest()
        {
            string hex = strToEncode.StringToEncodedString(Core.Security.HashEncoding.Hex);
            Console.WriteLine(hex);

            Assert.That(hex, Is.Not.Empty);
            Assert.That(hex, Is.Not.EqualTo(strToEncode));
            Assert.That(hex, Is.EqualTo(hexEncoded));
        }

        [Test]
        public void ConvertBytesToBase64Test()
        {
            string base64 = strToEncode.StringToEncodedString(Core.Security.HashEncoding.Base64);
            byte[] bytes = base64.BytesFromString();

            string converted = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(converted);

            Assert.That(converted, Is.Not.Empty);
            Assert.That(converted, Is.Not.EqualTo(strToEncode));
            Assert.That(converted, Is.EqualTo(base64Encoded));
        }

        [Test]
        public void ConvertBytesToHexTest()
        {
            string hex = strToEncode.StringToEncodedString(Core.Security.HashEncoding.Hex);
            byte[] bytes = hex.BytesFromString();

            string converted = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(converted);

            Assert.That(converted, Is.Not.Empty);
            Assert.That(converted, Is.Not.EqualTo(strToEncode));
            Assert.That(converted, Is.EqualTo(hexEncoded));
        }
    }
}
