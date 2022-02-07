using LootViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigParsers.Loot;
using ConfigParsers.Loot.Data;
using System.IO;

namespace LootViewer.Services
{
    public class Database
    {
        public LootParser? LootData { get => _lootData; }
        private LootParser? _lootData;

        //private List<Models.Item> _items = new();

        public Database()
        {
        }

        public List<LootItem> OpenPath(string? configFilePath)
        {
            if (!string.IsNullOrWhiteSpace(configFilePath))
            {
                var lootXmlPath = Path.Combine(configFilePath, "loot.xml");
                if (!string.IsNullOrWhiteSpace(lootXmlPath) && File.Exists(lootXmlPath))
                {
                    _lootData = new LootParser(lootXmlPath);
                    var items = new List<LootItem>();
                    foreach (var item in _lootData.Data.Items)
                    {
                        items.Add(new LootItem(item.Key));
                    }
                    return items;
                };
            }
            _lootData = null;
            return new List<LootItem>();
        }

    }
}
