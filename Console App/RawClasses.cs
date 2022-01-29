using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// Classes which are used for Deserialization of the loot.xml file
/// These classes are only temporarily used...
/// ... a further pass is made to convert these "Raw" classes into something more easily processed
/// </summary>
namespace _7DTD_Loot_Parser
{
    [XmlRoot(ElementName = "lootcontainers")]
    public class RawContainers
    {
        [XmlElement(ElementName = "lootgroup")]
        public List<RawGroup> Groups { get; set; }

        [XmlElement(ElementName = "lootcontainer")]
        public List<RawContainer> Containers { get; set; }
    }

    public class RawContainer
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "item")]
        public List<RawItem> Entries { get; set; }
    }

    public class RawGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        //[XmlAttribute("count")]
        //public string Count { get; set; }

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }

        [XmlElement(ElementName = "item")]
        public List<RawItem> Items { get; set; }

    }

    public class RawItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("group")]
        public string Group { get; set; }

        //[XmlAttribute("loot_prob_template")]
        //public string ProbTemplate { get; set; }

        //[XmlAttribute("mods")]
        //public string Mods { get; set; }

        //[XmlAttribute("mod_chance")]
        //public string ModChance { get; set; }

        //[XmlAttribute("prob")]
        //public string Prob { get; set; }
    }

    [XmlRoot(ElementName = "blocks")]
    public class RawBlocks
    {
        [XmlElement(ElementName = "block")]
        public List<RawBlock> Blocks { get; set; }
    }

    public class RawBlock
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "property")]
        public List<RawBlockProperty> Properties { get; set; }
    }

    public class RawBlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
