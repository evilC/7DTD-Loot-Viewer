using LootViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigParsers.Loot;
using ConfigParsers.Loot.Data;

namespace LootViewer.Services
{
    public class Database
    {
        public LootParser loot { get; }

        private List<Models.Item> _items = new List<Models.Item>();
        //private List<Container> _containers = new List<Container>();
        //private ItemContainerFinder rw;


        public Database(string lootXmlPath)
        {
            loot = new LootParser(lootXmlPath);
            foreach (var item in loot.Data.Items)
            {
                _items.Add(new Models.Item(item.Key));
            }
        }

        public IEnumerable<Models.Item> GetItems() => _items;

        //public IEnumerable<Container> GetContainers() => _containers;
    }
}
