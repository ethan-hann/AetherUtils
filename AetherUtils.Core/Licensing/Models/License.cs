// License.cs : AetherUtils
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
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Standard.Licensing;

namespace AetherUtils.Core.Licensing.Models;

/// <summary>
///     Represents a license for an application.
/// </summary>
[XmlRoot(ElementName = "License")]
[TypeConverter(typeof(ExpandableObjectConverter))]
public sealed class License : ExpandableObjectConverter
{
    /// <summary>
    ///     Create a new blank license model.
    /// </summary>
    public License() { }

    /// <summary>
    ///     Create a new License model using an existing <see cref="Standard.Licensing.License" />.
    /// </summary>
    /// <param name="existing">The existing license to create the model from.</param>
    public License(Standard.Licensing.License existing)
    {
        Id = existing.Id.ToString();
        Type = existing.Type;
        Expiration = existing.Expiration;
        Quantity = existing.Quantity;

        foreach (var feature in existing.ProductFeatures.GetAll())
            ProductFeatures.Add(new Feature(feature.Key, feature.Value));

        Customer = new Customer
        {
            Name = existing.Customer.Name,
            Email = existing.Customer.Email,
            Company = existing.Customer.Company
        };
        Signature = existing.Signature;
    }

    /// <summary>
    ///     The unique ID of this license.
    /// </summary>
    [XmlAttribute(AttributeName = "Id")]
    [Browsable(false)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The <see cref="LicenseType" /> for this license.
    /// </summary>
    [XmlElement(ElementName = "Type")]
    [Browsable(true)]
    [Category("License")]
    [DisplayName("License Type")]
    public LicenseType Type { get; set; } = LicenseType.Trial;

    /// <summary>
    ///     The date that this license should expire at;
    ///     has no effect if <see cref="Type" /> is <see cref="LicenseType.Trial" />.
    /// </summary>
    [XmlElement(ElementName = "Expiration")]
    [Browsable(true)]
    [Category("License")]
    [DisplayName("Expiration Date")]
    public DateTime Expiration { get; set; } = DateTime.Now.Date;

    /// <summary>
    ///     The number of uses allowed for this license.
    /// </summary>
    [XmlElement(ElementName = "Quantity")]
    [Browsable(true)]
    [Category("License")]
    [DisplayName("License Quantity")]
    public int Quantity { get; set; }

    /// <summary>
    ///     A <see cref="List{T}" /> of <see cref="Feature" />s for that are enabled with this license.
    /// </summary>
    [XmlArray(ElementName = "ProductFeatures")]
    [XmlArrayItem("Feature")]
    [Browsable(false)]
    [Category("License")]
    [DisplayName("Product Features")]
    public List<Feature> ProductFeatures { get; set; } = [];

    /// <summary>
    ///     The <see cref="Customer" /> associated with this license.
    /// </summary>
    [XmlElement(ElementName = "Customer")]
    [Browsable(false)]
    public Customer Customer { get; set; } = new();

    /// <summary>
    ///     The signature for this license. Auto-generated when signed with a private key.
    /// </summary>
    [XmlElement(ElementName = "Signature")]
    [Browsable(false)]
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    ///     Get a string that represents this license:<br />
    ///     ID: <br />
    ///     Type: <br />
    ///     Expiration: <br />
    ///     Quantity: <br />
    ///     Customer Name: <br />
    ///     Customer Email:
    /// </summary>
    /// <returns>A string representing this <see cref="License" />.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb = sb.AppendLine($"ID: {Id}")
            .AppendLine($"Type: {Type.ToString()}")
            .AppendLine($"Expiration: {Expiration.ToString(CultureInfo.InvariantCulture)}")
            .AppendLine($"Quantity: {Quantity}")
            .AppendLine($"Customer: {Customer.Name}")
            .AppendLine($"Customer Email: {Customer.Email}");
        return sb.ToString();

    }
}