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
        private List<TodoItem> _todo = new List<TodoItem>();

        public Database(string lootXmlPath)
        {
            var loot = new LootParser(lootXmlPath);
            //_items = loot.Data.Items;
            foreach (var item in loot.Data.Items)
            {
                _todo.Add(new TodoItem { Description = item.Key });
            }
        }

        //public IEnumerable<TodoItem> GetItems() => new List<TodoItem>()
        //{
        //    new TodoItem { Description = "Walk the dog" },
        //    new TodoItem { Description = "Buy some milk" },
        //    new TodoItem { Description = "Learn Avalonia", IsChecked = true },
        //};

        public IEnumerable<TodoItem> GetItems() => _todo;

    }
}
