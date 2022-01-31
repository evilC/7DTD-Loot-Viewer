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
namespace _7DTD_Loot_Parser.RawClasses
{
    [XmlRoot(ElementName = "lootcontainers")]
    public class _RootNode
    {
        [XmlElement(ElementName = "lootgroup")]
        public List<Group> Groups { get; set; }

        [XmlElement(ElementName = "lootcontainer")]
        public List<Container> Containers { get; set; }

        [XmlElement(ElementName = "lootprobtemplates")]
        public List<LootProbTemplateBase> LootProbTemplateBase { get; set; }
    }

    public class Container
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Entries { get; set; }
    }

    public class Group
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        //[XmlAttribute("count")]
        //public string Count { get; set; }

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }

    }

    public class Item
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
}
