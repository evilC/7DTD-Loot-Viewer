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
        public string ConfigFilePath { get; set; } = @"C:\Program Files (x86)\Steam\SteamApps\Common\7 Days To Die\Data\Config";
    }
}
