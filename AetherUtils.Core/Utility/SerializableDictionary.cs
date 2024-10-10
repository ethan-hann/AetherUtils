using System.ComponentModel;
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
    /// Also implements <see cref="INotifyPropertyChanged"/> and <see cref="IEquatable{T}"/> for comparisons.
    /// <para>This derived type from <see cref="Dictionary{TKey, TValue}"/> does not allow <c>null</c> for its values.</para>
    /// </summary>
    /// <typeparam name="TKey">The <see cref="Type"/> for the keys in this dictionary.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type"/> for the values in this dictionary.</typeparam>
    [XmlRoot("dictionary")]
    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : 
        Dictionary<TKey, TValue>, IXmlSerializable, INotifyPropertyChanged, ICloneable, IEquatable<SerializableDictionary<TKey, TValue>> where TKey : notnull
    {
        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new(typeof(TKey));
            XmlSerializer valueSerializer = new(typeof(TValue));

            var wasEmpty = reader.IsEmptyElement;

            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey?)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (TValue?)valueSerializer.Deserialize(reader);
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
            OnPropertyChanged(nameof(Count));
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new(typeof(TKey));
            XmlSerializer valueSerializer = new(typeof(TValue));

            foreach (var key in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            
            OnPropertyChanged(nameof(Count));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public object Clone()
        {
            var clone = new SerializableDictionary<TKey, TValue>();
            foreach (var pair in this)
                clone.Add(pair.Key, pair.Value);
            return clone;
        }

        public bool Equals(SerializableDictionary<TKey, TValue>? other)
        {
            return other != null && Keys.SequenceEqual(other.Keys) && Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SerializableDictionary<TKey, TValue>);
        }

        public override int GetHashCode() => HashCode.Combine(Keys);
    }
}