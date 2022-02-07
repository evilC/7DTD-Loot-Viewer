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

        private List<Models.Item> _items = new();

        public Database()
        {
        }

        public void OpenPath(string lootXmlPath)
        {
            if (!File.Exists(lootXmlPath)) return;
            _lootData = new LootParser(lootXmlPath);
            foreach (var item in _lootData.Data.Items)
            {
                _items.Add(new Models.Item(item.Key));
            }
        }

        public IEnumerable<Models.Item> GetItems() => _items;
    }
}
