using Standard.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AetherUtils.Licensing.Models
{
    /// <summary>
    /// Represents an end-user license for a product.
    /// </summary>
    [XmlRoot(ElementName = "License")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class License : ExpandableObjectConverter
    {
        /// <summary>
        /// The Guid of this license.
        /// </summary>
        [XmlAttribute(AttributeName = "Id")]
        [Browsable(false)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The license type: see <see cref="LicenseType"/> for options.
        /// </summary>
        [XmlElement(ElementName = "Type")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("License Type")]
        public LicenseType Type { get; set; } = LicenseType.Standard;

        /// <summary>
        /// The expiration date, or <c>null</c> if no expiration date for this license.
        /// </summary>
        [XmlElement(ElementName = "Expiration")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("Expiration Date")]
        public DateTime? Expiration { get; set; } = null;

        /// <summary>
        /// The quantity of uses this license covers.
        /// </summary>
        [XmlElement(ElementName = "Quantity")]
        [Browsable(true)]
        [Category("License")]
        [DisplayName("License Quantity")]
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// A list of product features that are actived via this license.
        /// </summary>
        [XmlArray(ElementName = "ProductFeatures")]
        [XmlArrayItem(typeof(Feature), ElementName = "Feature")]
        [Browsable(false)]
        [Category("License")]
        [DisplayName("Product Features")]
        public List<Feature> ProductFeatures { get; set; } = [];

        /// <summary>
        /// The <see cref="Models.Customer"/> this license is assigned to, or <c>null</c> if no customer assigned.
        /// </summary>
        [XmlElement(ElementName = "Customer")]
        [Browsable(false)]
        public Customer? Customer { get; set; } = null;

        /// <summary>
        /// The signature of the license after signed with private key.
        /// </summary>
        [XmlElement(ElementName = "Signature")]
        [Browsable(false)]
        public string Signature {  get; set; } = string.Empty;

        public License() { }
    }
}
