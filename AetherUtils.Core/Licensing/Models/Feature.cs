using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AetherUtils.Core.Licensing.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Feature")]
    public class Feature
    {
        [XmlAttribute(AttributeName = "name")]
        [Browsable(true)]
        [Category("Feature")]
        public string Name { get; set; } = string.Empty;

        [XmlText]
        [Browsable(true)]
        [Category("Feature")]
        public string Text { get; set; } = string.Empty;

        public Feature() { }

        public Feature(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}
