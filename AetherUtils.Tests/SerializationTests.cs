// SerializationTests.cs : AetherUtils
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

using AetherUtils.Core.Extensions;
using AetherUtils.Core.Licensing.Models;
using AetherUtils.Core.Security;
using AetherUtils.Core.Utility;

namespace AetherUtils.Tests;

public class SerializationTests
{
    private readonly License obj = new();
    private readonly BasicData obj2 = new();

    [Test]
    public void SerializeLicenseXmlTest()
    {
        var _xml = obj.SerializeXml();
        Assert.That(_xml, Is.Not.Null.And.Not.Empty);

        Console.WriteLine(_xml);

        var deserialized = _xml.DeserializeXml<License>();
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized.Id, Is.EqualTo(obj.Id));
    }

    [Test]
    public void SerializeBasicDataXmlTest()
    {
        obj2.dictionary.Add(1, "First item");
        obj2.dictionary.Add(2, "Second item");

        var _xml = obj2.SerializeXml();
        Assert.That(_xml, Is.Not.Null.And.Not.Empty);

        Console.WriteLine(_xml);

        var deserialized = _xml.DeserializeXml<BasicData>();
        Assert.That(deserialized, Is.Not.Null);
    }

    [Test]
    public void SerializeBasicDataJsonTest()
    {
        obj2.dictionary.Add(3, "Third item");
        obj2.dictionary.Add(4, "Fourth item");

        var json = obj2.SerializeJson();
        Assert.That(json, Is.Not.Null.And.Not.Empty);

        Console.WriteLine(json);

        var deserialized = json.DeserializeJson<BasicData>();
        Assert.That(deserialized, Is.Not.Null);
    }

    [Test]
    public void SerializeSecretQaJsonTest()
    {
        List<SecretQa> list = [];

        for (var i = 0; i < 10; i++)
            list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

        var json = list.SerializeJson();
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
        var _xml = dict.SerializeXml();

        Console.WriteLine(_xml);

        //Check empty and null
        Assert.That(_xml, Is.Not.Null.And.Not.Empty);

        //Deserialize back to object from XML
        var deserialized = _xml.DeserializeXml<SerializableDictionary<int, string>>();

        //Check if null deserialization occured.
        Assert.That(deserialized, Is.Not.Null);

        //Loop and compare new keys to items from original dictionary.
        foreach (var key in deserialized.Keys)
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
        foreach (var key in deserialized.Keys)
            Assert.That(dict[key], Is.EqualTo(deserialized[key]));
    }
}

/// <summary>
///     Test class for basic data serialization/deserialization test.
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