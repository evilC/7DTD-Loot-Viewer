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
        public Dictionary<string, string> ParseLocalizationFile()
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
            using (var parser = new TextFieldParser("Localization.txt"))
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
                        if (fields[1] == "items" || fields[1] == "item_modifiers" || fields[1] == "blocks" || fields[1] == "vehicles")
                        {
                            //Debug.WriteLine($"Writing Item {fields[0]} = {fields[5]}");
                            var displayName = fields[5];
                            displayName = displayName.Replace("\"", "");
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

    }

    public class LocalizationData
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
