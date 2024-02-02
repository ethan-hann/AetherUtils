using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    [XmlRoot(ElementName = "Customer")]
    public class Customer
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement(ElementName = "Company")]
        public string Company { get; set; } = string.Empty;

        [XmlElement(ElementName = "Email")]
        public string Email { get; set; } = string.Empty;

        public Customer() { }
    }
}
