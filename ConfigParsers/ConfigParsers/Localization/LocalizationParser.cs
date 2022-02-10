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
        /// <summary>
        /// It takes a LONG TIME to parse the localization file, so only do it once and cache the results
        /// </summary>
        private string _cacheFile = "DisplayNames.json";

        public SortedDictionary<string, string> GetDisplayNames(string configFilePath)
        {
            //var containerNames = GetContainerNames(configFilePath);
            //Dictionary<string, string> data;

            // If we already have cached data, load that
            if (File.Exists(_cacheFile))
            {
                string jsonString = File.ReadAllText(_cacheFile);
                var cachedData = JsonSerializer.Deserialize<SortedDictionary<string, string>>(jsonString);
                if (cachedData == null) throw new Exception("JSON serialization failed");
                return cachedData;
            }

            var localizationFilePath = BuildLocalizationFilePath(configFilePath);
            if (!File.Exists(localizationFilePath)) return new SortedDictionary<string, string>();
            // Build data
            var data = new SortedDictionary<string, string>();
            using (var parser = new TextFieldParser(localizationFilePath))
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
            File.WriteAllText(_cacheFile, JsonSerializer.Serialize(data, opt));
            return data;
        }

        private string BuildLocalizationFilePath(string configFilePath)
        {
            return Path.Combine(new string[] { configFilePath, "Localization.txt" });
        }

        public void ClearCache()
        {
            if (File.Exists(_cacheFile)) File.Delete(_cacheFile);
        }

        public bool LocalizationFileExists(string configFilePath)
        {
            return File.Exists(BuildLocalizationFilePath(configFilePath));
        }
    }

}
