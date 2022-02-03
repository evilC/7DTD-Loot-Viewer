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
namespace ConfigParsers.Loot.XmlClasses
{
    /// <summary>
    /// This class is passed to the Deserializer and lets it interpret the root node of the XML
    /// </summary>
    [XmlRoot(ElementName = "lootcontainers")]
    public class Root
    {
        [XmlElement(ElementName = "lootgroup")]
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// When Deserializing the XML, you get a list of groups, rather than a dictionary...
        /// So BuildGroupDictionary converts this into a Dictionary after parsing, for easy group lookup
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, Group> GroupsDictionary { get; set; } = new Dictionary<string, Group>();

        [XmlElement(ElementName = "lootcontainer")]
        public List<Group> Containers { get; set; } = new List<Group>();

        [XmlElement(ElementName = "lootprobtemplates")]
        public List<RootProbTemplate> LootProbTemplateBase { get; set; } = new List<RootProbTemplate>();

        [XmlElement(ElementName = "lootqualitytemplates")]
        public List<RootQualTemplate> LootQualTemplateBase { get; set; } = new List<RootQualTemplate>();

        public void BuildGroupDictionary()
        {
            GroupsDictionary = Groups.ToDictionary(i => i.Name);
            foreach (var container in Containers)
            {
                if (IsIgnoredContainer(container.Name)) continue;
                GroupsDictionary.Add(container.Name, container);
            }
        }

        // These names appear in loot.xml as both Containers and Groups
        // They seem to be to do with testing / twitch and not normal gameplay, so safe to ignore?
        public bool IsIgnoredContainer(string name)
        {
            return name == "weaponTestLoot"
                    || name == "toolTestLoot"
                    || name.StartsWith("twitch_");
        }
    }
}
