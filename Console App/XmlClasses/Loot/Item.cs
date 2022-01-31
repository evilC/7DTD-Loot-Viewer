using System.Xml.Serialization;

/// <summary>
/// Classes which are used for Deserialization of the loot.xml file
/// These classes are only temporarily used...
/// ... a further pass is made to convert these "Raw" classes into something more easily processed
/// </summary>
namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class Item
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("group")]
        public string Group { get; set; }

        [XmlAttribute("count")]
        public string Count { get; set; }

        [XmlAttribute("loot_prob_template")]
        public string ProbTemplate { get; set; }

        [XmlAttribute("prob")]
        public string Prob { get; set; }
        //[XmlAttribute("mods")]
        //public string Mods { get; set; }

        //[XmlAttribute("mod_chance")]
        //public string ModChance { get; set; }
    }
}
