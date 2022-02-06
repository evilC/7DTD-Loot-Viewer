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
        private List<Models.Item> _items = new List<Models.Item>();
        private List<Container> _containers = new List<Container>();
        private ItemContainerFinder rw;

        public Database(string lootXmlPath)
        {
            var loot = new LootParser(lootXmlPath);
            rw = new ItemContainerFinder(loot.Data);
            //_items = loot.Data.Items;
            foreach (var item in loot.Data.Items)
            {
                _items.Add(new Models.Item { Description = item.Key });
            }

            _containers = new List<Container>();
            var results = rw.GetItemContainers("meleeToolSalvageT2Ratchet");
            foreach (var container in results.ContainerResults)
            {
                var cr = container.Value;
                var probCalc = new ProbabilityCalculator(cr);
                var prob = probCalc.CalculateProbability(102);
                _containers.Add(new Container() { Name = container.Key, Prob = prob });
            }
        }

        public IEnumerable<Models.Item> GetItems() => _items;

        public IEnumerable<Container> GetContainers() => _containers;
    }
}
