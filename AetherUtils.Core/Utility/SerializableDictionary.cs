// SerializableDictionary.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AetherUtils.Core.Utility;
//Implemented based on: https://asp-blogs.azurewebsites.net/pwelter34/444961
//Modified to add exceptions and .Net 8.0 features.

/// <summary>
///     Represents a generic key-value dictionary that is serializable via XML serialization.
///     Also implements <see cref="INotifyPropertyChanged" /> and <see cref="IEquatable{T}" /> for comparisons.
///     <para>This derived type from <see cref="Dictionary{TKey, TValue}" /> does not allow <c>null</c> for its values.</para>
/// </summary>
/// <typeparam name="TKey">The <see cref="Type" /> for the keys in this dictionary.</typeparam>
/// <typeparam name="TValue">The <see cref="Type" /> for the values in this dictionary.</typeparam>
[XmlRoot("dictionary")]
[Serializable]
public sealed class SerializableDictionary<TKey, TValue> :
    Dictionary<TKey, TValue>, IXmlSerializable, INotifyPropertyChanged, ICloneable, IEquatable<SerializableDictionary<TKey, TValue>> where TKey : notnull
{
    /// <inheritdoc />
    public object Clone()
    {
        var clone = new SerializableDictionary<TKey, TValue>();
        foreach (var pair in this)
            clone.Add(pair.Key, pair.Value);
        return clone;
    }

    /// <inheritdoc />
    public bool Equals(SerializableDictionary<TKey, TValue>? other) =>
        other != null && Keys.SequenceEqual(other.Keys) && Values.SequenceEqual(other.Values);

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc />
    public XmlSchema? GetSchema() => null;

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SerializableDictionary<TKey, TValue>);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Keys);
}