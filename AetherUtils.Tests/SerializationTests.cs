using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security;
using AetherUtils.Core.Utility;

namespace AetherUtils.Tests
{
    public class SerializationTests
    {
        private License obj = new License();
        private BasicData obj2 = new BasicData();

        [Test]
        public void SerializeLicenseXmlTest()
        {
            string _xml = obj.SerializeXml();
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            Console.WriteLine(_xml);

            License? deserialized = _xml.DeserializeXml<License>();
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized.Id, Is.EqualTo(obj.Id));
        }

        [Test]
        public void SerializeBasicDataXmlTest()
        {
            obj2.dictionary.Add(1, "First item");
            obj2.dictionary.Add(2, "Second item");

            string _xml = obj2.SerializeXml();
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            Console.WriteLine(_xml);

            BasicData? deserialized = _xml.DeserializeXml<BasicData>();
            Assert.That(deserialized, Is.Not.Null);
        }

        [Test]
        public void SerializeBasicDataJsonTest()
        {
            obj2.dictionary.Add(3, "Third item");
            obj2.dictionary.Add(4, "Fourth item");

            string json = obj2.SerializeJson();
            Assert.That(json, Is.Not.Null.And.Not.Empty);

            Console.WriteLine(json);

            BasicData? deserialized = json.DeserializeJson<BasicData>();
            Assert.That(deserialized, Is.Not.Null);
        }

        [Test]
        public void SerializeSecretQaJsonTest()
        {
            List<SecretQa> list = [];

            for (var i = 0; i < 10; i++)
                list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

            string json = list.SerializeJson();
            Console.WriteLine(json);
            
            //Check empty and null
            Assert.That(json, Is.Not.Null.And.Not.Empty);
            
            //Deserialize back to object from XML
            var deserialized = json.DeserializeJson<List<SecretQa>>();

            //Check if null deserialization occured.
            Assert.That(deserialized, Is.Not.Null);

            //Loop and compare new keys to items from original dictionary.
            for (var i = 0; i < list.Count; i++)
            {
                Assert.That(list[i].Question, Is.EqualTo(deserialized[i].Question));
                Assert.That(list[i].Answer, Is.EqualTo(deserialized[i].Answer));
            }
        }

        [Test]
        public void SerializeDictionaryXmlTest()
        {
            //Create dictionary and populate
            SerializableDictionary<int, string> dict = [];
            dict.Add(1, "First item");
            dict.Add(2, "Second item");
            dict.Add(3, "Third item");

            //Serialize it to an XML string
            string _xml = dict.SerializeXml();

            Console.WriteLine(_xml);

            //Check empty and null
            Assert.That(_xml, Is.Not.Null.And.Not.Empty);

            //Deserialize back to object from XML
            var deserialized = _xml.DeserializeXml<SerializableDictionary<int, string>>();

            //Check if null deserialization occured.
            Assert.That(deserialized, Is.Not.Null);

            //Loop and compare new keys to items from original dictionary.
            foreach (int key in deserialized.Keys)
                Assert.That(dict[key], Is.EqualTo(deserialized[key]));
        }

        [Test]
        public void SerializeDictionaryJsonTest()
        {
            //Create dictionary and populate
            SerializableDictionary<int, string> dict = [];
            dict.Add(1, "First item");
            dict.Add(2, "Second item");
            dict.Add(3, "Third item");

            //Serialize it to an XML string
            var json = dict.SerializeJson();

            Console.WriteLine(json);

            //Check empty and null
            Assert.That(json, Is.Not.Null.And.Not.Empty);

            //Deserialize back to object from XML
            var deserialized = json.DeserializeJson<SerializableDictionary<int, string>>();

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
