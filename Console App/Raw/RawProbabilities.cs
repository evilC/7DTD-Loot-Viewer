using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.RawClasses
{
    public class LootProbTemplateBase
    {
        [XmlElement(ElementName = "lootprobtemplate")]
        public List<LootProbTemplate> LootProbTemplates { get; set; }
    }

    public class LootProbTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "loot")]
        public List<LootProbTemplateItem> LootProbTemplateItems { get; set; }
    }

    public class LootProbTemplateItem
    {
        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("prob")]
        public decimal Prob { get; set; }
    }
}
