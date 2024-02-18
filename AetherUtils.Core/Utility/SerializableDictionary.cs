using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace AetherUtils.Core.Utility
{
    //Implemented based on: https://asp-blogs.azurewebsites.net/pwelter34/444961
    //Modified to add exceptions and .Net 8.0 features.

    /// <summary>
    /// Represents a generic key-value dictionary that is serializable via XML serialization.
    /// <para>This derived type from <see cref="Dictionary{TKey, TValue}"/> does not allow <c>null</c> for its values.</para>
    /// </summary>
    /// <typeparam name="TKey">The <see cref="Type"/> for the keys in this dictionary.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type"/> for the values in this dictionary.</typeparam>
    [XmlRoot("dictionary")]
    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : 
        Dictionary<TKey, TValue>, IXmlSerializable where TKey : notnull where TValue : notnull
    {
        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new(typeof(TKey));
            XmlSerializer valueSerializer = new(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey? key = (TKey?)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue? value = (TValue?)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                if (key == null)
                    throw new NullReferenceException($"The key of the dictionary was null: {nameof(key)}");
                if (value == null)
                    throw new NullReferenceException($"The value of the dictionary key was null: {nameof(value)}");

                Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new(typeof(TKey));
            XmlSerializer valueSerializer = new(typeof(TValue));

            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}