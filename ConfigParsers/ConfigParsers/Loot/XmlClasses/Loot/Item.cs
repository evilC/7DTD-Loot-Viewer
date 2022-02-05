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
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("group")]
        public string Group { get; set; } = string.Empty;

        [XmlAttribute("count")]
        public string Count { get; set; } = string.Empty;

        [XmlAttribute("loot_prob_template")]
        public string ProbTemplate { get; set; } = string.Empty;

        [XmlAttribute("prob")]
        public string Prob { get; set; } = string.Empty;

        [XmlAttribute("force_prob")]
        public string ForceProb { get; set; } = string.Empty;


    }
}
