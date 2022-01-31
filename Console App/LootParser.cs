using System.Xml.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace _7DTD_Loot_Parser
{

    internal class LootParser
    {
        public LootParser(string configFilePath)
        {
            // Deserialize the XML file into the Raw classes
            var xmlRoot = ObjectDeserializer.DeserializeToObject<XmlClasses.Loot.Root>
                (Path.Combine(new string[] { configFilePath, "loot.xml" }));
            // Convert the Loot Groups into a Dictionary, indexed by name of Loot Group
            xmlRoot.BuildGroupDictionary();
            // Now we have the raw data from the XML, attempt to parse all the entries and build a data tree

            var loot = new Data.Loot(xmlRoot);

            // We don't need the original XML data any more, so free it from memory
            xmlRoot = null;

            // Code to do useful things with the data here
        }
    }

}
