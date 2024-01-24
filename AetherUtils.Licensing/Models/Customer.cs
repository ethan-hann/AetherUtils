using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AetherUtils.Licensing.Models
{
    /// <summary>
    /// Represents a customer with whom a license is assigned to.
    /// </summary>
    [XmlRoot(ElementName = "Customer", IsNullable = true)]
    public class Customer
    {
        /// <summary>
        /// The name of the customer.
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The company this customer is associated with.
        /// </summary>
        [XmlElement(ElementName = "Company")]
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the customer.
        /// </summary>
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; } = string.Empty;

        public Customer() { }
    }
}
