using AetherUtils.Core.Extensions;
using AetherUtils.Core.Utility;

namespace AetherUtils.Tests
{
    public class ExtensionTests
    {
        public SerializableDictionary<int, string> dictionary;

        [SetUp]
        public void SetUp()
        {
            //Create dictionary and populate
            dictionary = [];
            dictionary.Add(1, "First item");
            dictionary.Add(2, "Second item");
            dictionary.Add(3, "Third item");
        }

        [Test]
        public void RenameKeyTest()
        {
            dictionary.RenameKey(1, 10);

            //After rename, key (10) should be equal to the first item in the dictionary.
            Assert.That(dictionary[10], Is.EqualTo("First item"));
        }

        [Test]
        public void RenameKeyTest100()
        {
            for (int i = 1; i <= 100; i++)
            {
                dictionary.RenameKey(1, 10); //Rename key to 10.

                //After rename, key (10) should be equal to the first item in the dictionary.
                Assert.That(dictionary[10], Is.EqualTo("First item"));

                dictionary.RenameKey(10, 1); //Rename key back to 1.
            }
        }

        [Test]
        public void RenameKeyTestException()
        {
            SerializableDictionary<int, string> dictionary2 = [];

            dictionary2.Add(1, "First item");
            dictionary2.Add(2, "Second item");
            dictionary2.Add(3, "Third item");

            //Assert that an exception is thrown if we try to rename a key with a name that already exists in the dictionary.
            Assert.Throws<ArgumentException>(() => dictionary2.RenameKey(1, 2));
        }
    }
}
