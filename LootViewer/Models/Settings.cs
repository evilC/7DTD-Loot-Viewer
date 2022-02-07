using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    // ToDo: This implementation is REALLY janky - need to improve
    public class Settings
    {
        private string _settingsFile = "settings";
        public string ConfigFilePath { get; set; } = @"C:\Program Files (x86)\Steam\SteamApps\Common\7 Days To Die\Data\Config";

        public Settings()
        {

        }

        public void Load()
        {
            if (!File.Exists(_settingsFile)) return;
            using (var fs = File.OpenRead(_settingsFile))
            {
                Settings? s = LoadFromStream(fs);
                if (s == null)
                {
                    return;
                }
                ConfigFilePath = s.ConfigFilePath;
            }
        }

        private Settings? LoadFromStream(FileStream stream)
        {
            return JsonSerializer.Deserialize<Settings>(stream);
        }

        public void Save()
        {
            using (var fs = File.OpenWrite(_settingsFile))
            {
                SaveToStream(this, fs);
            }
        }

        private static void SaveToStream(Settings data, Stream stream)
        {
            JsonSerializer.Serialize(stream, data);
        }

    }
}
