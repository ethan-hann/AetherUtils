// // Customer.cs : AetherUtils
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

using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models;

/// <summary>
///     Represents a customer for a license.
/// </summary>
[XmlRoot(ElementName = "Customer")]
public sealed class Customer
{
    /// <summary>
    ///     Create a new, default customer.
    /// </summary>
    public Customer() { }

    /// <summary>
    ///     The customer's name.
    /// </summary>
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     The customer's company, if any.
    /// </summary>
    [XmlElement(ElementName = "Company")]
    public string Company { get; set; } = string.Empty;

    /// <summary>
    ///     The customer's email address.
    /// </summary>
    [XmlElement(ElementName = "Email")]
    public string Email { get; set; } = string.Empty;
}