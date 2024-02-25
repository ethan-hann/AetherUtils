using System.ComponentModel;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    /// <summary>
    /// Represents a customer for a license.
    /// </summary>
    [XmlRoot(ElementName = "Customer")]
    public sealed class Customer
    {
        /// <summary>
        /// The customer's name.
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The customer's company, if any.
        /// </summary>
        [XmlElement(ElementName = "Company")]
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// The customer's email address.
        /// </summary>
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Create a new, default customer.
        /// </summary>
        public Customer() { }
    }
}
