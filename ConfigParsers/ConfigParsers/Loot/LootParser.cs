using ConfigParsers.Common;
using ConfigParsers.Loot.Data;

namespace ConfigParsers.Loot
{
    public class LootParser
    {
        public Data.LootParser Data { get; set; }

        public LootParser(string lootXmlPath)
        {
            // Deserialize the XML file into the Raw classes
            var xmlRoot = ObjectDeserializer.DeserializeToObject<XmlClasses.Root>
                (lootXmlPath);
            // Convert the Loot Groups into a Dictionary, indexed by name of Loot Group
            xmlRoot.BuildGroupDictionary();
            // Now we have the raw data from the XML, attempt to parse all the entries and build a data tree

            Data = new Data.LootParser(xmlRoot);

            // We don't need the original XML data any more, so free it from memory
            xmlRoot = null;

            // Code to do useful things with the data here
        }
    }
}
