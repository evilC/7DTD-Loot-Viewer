using ConfigParsers.Common;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Text.Json;

namespace ConfigParsers.Localization
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
            //Dictionary<string, string> data;

            // If we already have cached data, load that
            if (File.Exists("ItemNames.json"))
            {
                string jsonString = File.ReadAllText("ItemNames.json");
                var cachedData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                if (cachedData == null) throw new Exception("JSON serialization failed");
            }

            // Build data
            var data = new Dictionary<string, string>();
            
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
                        var val = parser.ReadFields();
                        if (val != null)
                        {
                            string[] fields = val;

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

}
