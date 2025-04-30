// // Feature.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using System.ComponentModel;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models;

/// <summary>
///     Represents a single feature for an application.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "Feature")]
public sealed class Feature
{
    /// <summary>
    ///     Create a new, default feature.
    /// </summary>
    public Feature() { }

    /// <summary>
    ///     Create a new feature with the specified name and text.
    /// </summary>
    /// <param name="name">The name of the feature.</param>
    /// <param name="text">The text associated with this feature.</param>
    public Feature(string name, string text)
    {
        Name = name;
        Text = text;
    }

    /// <summary>
    ///     The feature name.
    /// </summary>
    [XmlAttribute(AttributeName = "name")]
    [Browsable(true)]
    [Category("Feature")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     The text associated with this feature.
    ///     This can be a description of the feature or what aspects of the application are enabled with this feature.
    /// </summary>
    [XmlText]
    [Browsable(true)]
    [Category("Feature")]
    public string Text { get; set; } = string.Empty;
}