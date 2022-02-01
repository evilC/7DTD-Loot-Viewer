using System.Xml.Serialization;

/// <summary>
/// XML lootcontainer item elements deserialize into this class
/// Note that this class can hold an Item or a Group...
/// So when this file is later parsed, it may turn into a Data.LootGroup or a Data.LootGroupItem class
/// </summary>
namespace ConfigParsers.Loot.XmlClasses
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
