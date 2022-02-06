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
        //private SortedDictionary<string, Item> _items;
        private List<Models.Item> _items = new List<Models.Item>();
        private List<Container> _containers = new List<Container>();

        public Database(string lootXmlPath)
        {
            var loot = new LootParser(lootXmlPath);
            //_items = loot.Data.Items;
            foreach (var item in loot.Data.Items)
            {
                _items.Add(new Models.Item { Description = item.Key });
            }

        }

        //public IEnumerable<TodoItem> GetItems() => new List<TodoItem>()
        //{
        //    new TodoItem { Description = "Walk the dog" },
        //    new TodoItem { Description = "Buy some milk" },
        //    new TodoItem { Description = "Learn Avalonia", IsChecked = true },
        //};

        public IEnumerable<Models.Item> GetItems() => _items;

        public IEnumerable<Container> GetContainers() => new List<Container>() {
            new Container() { Name = "C1", Prob = 1.23M },
            new Container() { Name = "C2", Prob = 2.34M }
        };
    }
}
