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

        public List<Models.Item> OpenPath(string? lootXmlPath)
        {
            if (string.IsNullOrEmpty(lootXmlPath) || !File.Exists(lootXmlPath))
            {
                if (_lootData != null)
                {
                    _lootData = null;
                    //_items.Clear();
                }
                return new List<Models.Item>();
            };
            _lootData = new LootParser(lootXmlPath);
            var items = new List<Models.Item>();
            foreach (var item in _lootData.Data.Items)
            {
                items.Add(new Models.Item(item.Key));
            }
            return items;
        }

        //public IEnumerable<Models.Item> GetItems() => _items;
    }
}
