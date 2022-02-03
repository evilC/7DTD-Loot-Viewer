using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigParsers.Loot.XmlClasses
{
    /// <summary>
    /// XML lootprobtemplates node deserializes into this class
    /// Dictionary will only ever contain one KVP
    /// </summary>
    public class RootProbTemplate
    {
        [XmlElement(ElementName = "lootprobtemplate")]
        public List<ProbTemplate> LootProbTemplates { get; set; } = new List<ProbTemplate>();
    }
}
