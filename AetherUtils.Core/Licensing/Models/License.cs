using Standard.Licensing;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    [XmlRoot(ElementName = "License")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class License : ExpandableObjectConverter
    {
        [XmlAttribute(AttributeName = "Id")]
        [Browsable(false)]
        public string Id { get; set; }

        [XmlElement(ElementName = "Type")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("License Type")]
        public LicenseType Type { get; set; } = LicenseType.Trial;

        [XmlElement(ElementName = "Expiration")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("Expiration Date")]
        public DateTime Expiration { get; set; } = DateTime.Now.Date;

        [XmlElement(ElementName = "Quantity")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("License Quantity")]
        public int Quantity { get; set; }

        [XmlArray(ElementName = "ProductFeatures")]
        [XmlArrayItem("Feature")]
        [Browsable(false)]
        [Category("License")]
        [DisplayName("Product Features")]
        public List<Feature> ProductFeatures { get; set; } = [];

        [XmlElement(ElementName = "Customer")]
        [Browsable(false)]
        public Customer Customer { get; set; } = new Customer();

        [XmlElement(ElementName = "Signature")]
        [Browsable(false)]
        public string Signature { get; set; } = string.Empty;

        public License() { }

        /// <summary>
        /// Get a string that represents this license object:<br/>
        /// 
        /// ID: <br/>
        /// Type: <br/>
        /// Expiration: <br/>
        /// Quantity: <br/>
        /// Customer Name: <br/>
        /// Customer Email:
        /// </summary>
        /// <returns>A string representing this <see cref="License"/>.</returns>
        // public override string ToString()
        // {
        //     var sb = new StringBuilder();
        //     sb = sb.AppendLine($"ID: {Id}")
        //         .AppendLine($"Type: {Type.ToString()}")
        //         .AppendLine($"Expiration: {Expiration.ToString(CultureInfo.InvariantCulture)}")
        //         .AppendLine($"Quantity: {Quantity}")
        //         .AppendLine($"Customer: {Customer.Name}")
        //         .AppendLine($"Customer Email: {Customer.Email}");
        //     return sb.ToString();
        //
        // }
    }
}
