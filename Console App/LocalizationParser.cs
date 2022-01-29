using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Text.Json;

namespace _7DTD_Loot_Parser
{
    /// <summary>
    /// Allows us to use Display Names (As seen in UI) for items or containers
    /// eg converts "resourceSewingKit" to "Sewing Kit"
    /// </summary>
    public class LocalizationParser
    {
        public Dictionary<string, string> GetDisplayNames(string configFilePath)
        {
            //var containerNames = GetContainerNames(configFilePath);

            Dictionary<string, string> data;

            // If we already have cached data, load that
            if (File.Exists("ItemNames.json"))
            {
                string jsonString = File.ReadAllText("ItemNames.json");
                data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                return data;
            }

            // Build data
            data = new Dictionary<string, string>();
            
            using (var parser = new TextFieldParser(Path.Combine(new string[] { configFilePath, "Localization.txt" })))
            {
                //parser.CommentTokens = new string[] { "#" };
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;

                // Skip over header line.
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] fields = parser.ReadFields();

                        // Ignore Descriptions
                        if (fields[0].EndsWith("Desc")) continue;
                        // Ignore localization entries that are not to do with items or containers
                        //if (fields[1] == "items" || fields[1] == "item_modifiers" || fields[1] == "blocks" || fields[1] == "vehicles")
                        if (fields[1] == "items" || fields[1] == "item_modifiers" || fields[1] == "blocks" || fields[1] == "vehicles")
                        {
                            //Debug.WriteLine($"Writing Item {fields[0]} = {fields[5]}");
                            var displayName = fields[5];

                            //string key;
                            //if (fields[2] == "Container")
                            //{
                            //    key = containerNames[fields[0]];
                            //}
                            //else
                            //{
                            //    key = fields[0];
                            //}
                            displayName = displayName.Replace("\"", "");
                            //data.Add(key, displayName);
                            data.Add(fields[0], displayName);
                        }
                    }
                    catch { };
                }
            }

            // Write out the localization data as JSON, so that we do not need to parse it on each run
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            File.WriteAllText("ItemNames.json", JsonSerializer.Serialize(data, opt));
            return data;
        }

        public Dictionary<string, string> GetContainerNames(string configFilePath)
        {
            /*
            To find Display name for containers
            1) Scan blocks.xml to find the LootList entry for that group:

            blocks.xml:
            <block name="cntMedicineCabinetClosed">
                <property name="Extends" value="cntMedicineCabinetOpen"/>
                <property name="CreativeMode" value="Player"/>
                <property name="Class" value="Loot"/>
            --> <property name="LootList" value="medicineCabinet"/>
                <property name="Model" value="#Entities/LootContainers?medicine_cabinet_closedPrefab.prefab"/>
            </block>

            2) Find the localization value in localization.txt
            localization.txt:
            cntMedicineCabinetClosed,blocks,Container,,,"Medicine Cabinet, Closed"
            */

            var containerNames = new Dictionary<string, string>();

            var rawBlocks = ObjectDeserializer.DeserializeToObject<RawBlocks>
                (Path.Combine(new string[] { configFilePath, "blocks.xml" }));

            foreach (var rawBlock in rawBlocks.Blocks)
            {
                if (rawBlock.Name == "cntMedicineCabinetClosed")
                {
                    var debug = 1;
                }
                foreach (var property in rawBlock.Properties)
                {
                    if (property.Name == "LootList")
                    {
                        if (containerNames.ContainsKey(rawBlock.Name))
                        {
                            Debug.WriteLine($"Key {rawBlock.Name} already has a value of {containerNames[rawBlock.Name]}, not adding {property.Value}");
                            //Debug.WriteLine($"Container {property.Value} already exists as {containerNames[property.Value]}, not adding {rawBlock.Name}");
                        }
                        else
                        {
                            containerNames.Add(rawBlock.Name, property.Value);
                        }
                        break;
                    }
                }
            }

            return containerNames;
        }
    }

}
