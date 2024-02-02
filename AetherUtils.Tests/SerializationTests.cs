using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Utility;

namespace AetherUtils.Tests
{
    public class SerializationTests
    {
        private License obj = new License();
        private BasicData obj2 = new BasicData();

        [Test]
        public void SerializeAndDeserializeXMLTest()
        {
            string _xml = obj.Serialize();
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            Console.WriteLine(_xml);

            License? deserialized = _xml.Deserialize<License>();
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized.Id, Is.EqualTo(obj.Id));
        }

        [Test]
        public void SerializeAndDeserializeBasicDataXMLTest()
        {
            obj2.dictionary.Add(1, "First item");
            obj2.dictionary.Add(2, "Second item");

            string _xml = obj2.Serialize();
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            Console.WriteLine(_xml);

            BasicData? deserialized = _xml.Deserialize<BasicData>();
            Assert.That(deserialized, Is.Not.Null);
        }

        [Test]
        public void SerializeDictionaryTest()
        {
            //Create dictionary and populate
            SerializableDictionary<int, string> dict = [];
            dict.Add(1, "First item");
            dict.Add(2, "Second item");
            dict.Add(3, "Third item");

            //Serialize it to an XML string
            string _xml = dict.Serialize();

            Console.WriteLine(_xml);

            //Check empty and null
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            //Deserialize back to object from XML
            var deserialized = _xml.Deserialize<SerializableDictionary<int, string>>();

            //Check if null deserialization occured.
            Assert.That(deserialized, Is.Not.Null);

            //Loop and compare new keys to items from original dictionary.
            foreach (int key in deserialized.Keys)
                Assert.That(dict[key], Is.EqualTo(deserialized[key]));
        }
    }

    /// <summary>
    /// Test class for basic data serialization/deserialization test.
    /// </summary>
    public class BasicData
    {
        public string Id { get; set; }
        public int num { get; set; }
        public double num2 { get; set; }
        public bool is_valid { get; set; }
        public List<string> strings { get; set; } = [];
        public SerializableDictionary<int, string> dictionary { get; set; } = [];
        public short shortVal { get; set; }
        private decimal privateDec { get; set; }
    }
}
