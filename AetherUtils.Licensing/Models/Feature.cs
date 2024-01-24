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
    /// Represents a product feature.
    /// </summary>
    [XmlRoot(ElementName = "Feature")]
    public class Feature
    {
        /// <summary>
        /// The name of this feature.
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        [Browsable(true)]
        [Category("Feature")]
        [DisplayName("Feature Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of this feature.
        /// </summary>
        [XmlText]
        [Browsable(true)]
        [Category("Feature")]
        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Create a new blank feature.
        /// </summary>
        public Feature() { }

        /// <summary>
        /// Create a new feature with the specified name and description.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Feature(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
