using System.ComponentModel;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    /// <summary>
    /// Represents a single feature for an application.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Feature")]
    public sealed class Feature
    {
        /// <summary>
        /// The feature name.
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        [Browsable(true)]
        [Category("Feature")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The text associated with this feature.
        /// This can be a description of the feature or what aspects of the application are enabled with this feature.
        /// </summary>
        [XmlText]
        [Browsable(true)]
        [Category("Feature")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Create a new, default feature.
        /// </summary>
        public Feature() { }

        /// <summary>
        /// Create a new feature with the specified name and text.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        /// <param name="text">The text associated with this feature.</param>
        public Feature(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}
