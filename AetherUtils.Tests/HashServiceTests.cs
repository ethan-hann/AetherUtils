using AetherUtils.Core.Security;
using AetherUtils.Core.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [SetUp]
        public void SetUp()
        {
            options = new HashOptions(64, new ReadOnlyPair<short, short>(500, 2000));
            service = service = new HashService(options);
            service.HashString(testString);
        }

        [Test]
        public void CreateHashTest()
        {
            Console.WriteLine(testString);
            Console.WriteLine(service.HashedString);

            //Assert that hashed string is not empty
            Assert.That(service.HashedString, Is.Not.Empty);
        }

        [Test]
        public void CompareHashTest()
        {
            Console.WriteLine(testString);
            Console.WriteLine(testString2);
            Console.WriteLine(service.HashedString);

            Assert.Multiple(() =>
            {
                //Assert that the testString2 is NOT equalvilent to the hashed string.
                Assert.That(service.CompareHash(testString2), Is.False);

                //Assert that the testString is equalvalent to the hashed string.
                Assert.That(service.CompareHash(testString), Is.True);
            });
        }
    }
}
