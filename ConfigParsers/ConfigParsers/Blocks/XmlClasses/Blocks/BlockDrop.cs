using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigParsers.Blocks.XmlClasses
{
    public class BlockDrop
    {
        [XmlAttribute("event")]
        public string Event { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string ResourceName { get; set; } = string.Empty;

        [XmlAttribute("prob")]
        public string Prob { get; set; } = string.Empty;

        [XmlAttribute("count")]
        public string Count { get; set; } = string.Empty;

        [XmlAttribute("tag")]
        public string Tag { get; set; } = string.Empty;
    }
}
