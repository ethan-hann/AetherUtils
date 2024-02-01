using Standard.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    [XmlRoot(ElementName = "License")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class License : ExpandableObjectConverter
    {
        [XmlAttribute(AttributeName = "Id")]
        [Browsable(false)]
        public string Id { get; set; } = "Default";

        [XmlElement(ElementName = "Type")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("License Type")]
        public LicenseType Type { get; set; }

        [XmlElement(ElementName = "Expiration")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("Expiration Date")]
        public DateTime Expiration { get; set; }

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
        public List<Feature> ProductFeatures { get; set; } = [new() { Name = "Core", Text = "Yes" }];

        [XmlElement(ElementName = "Customer")]
        [Browsable(false)]
        public Customer Customer { get; set; } = new Customer();

        [XmlElement(ElementName = "Signature")]
        [Browsable(false)]
        public string Signature { get; set; } = string.Empty;

        public License() { }
    }
}
