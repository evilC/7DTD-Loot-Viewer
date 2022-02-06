using Avalonia.Controls.Selection;
using ConfigParsers.Loot;
using LootViewer.Models;
using LootViewer.Services;
using LootViewer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace LootViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Database _db;

        public LootLevelView LootLevelView { get; set; }
        public int LootLevel { get; set; }

        public ItemListView ItemListView { get; set; }
        public ObservableCollection<Item> Items { get; }
        public SelectionModel<Item> ItemSelection { get; }


        public ContainerListView ContainerListView { get; set; }
        public ObservableCollection<Container> Containers { get; }

        public MainWindowViewModel()
        {
            _db = new Database(@"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml");

            LootLevelView = new LootLevelView();
            LootLevel = 1;

            ItemListView = new ItemListView();
            Items = new ObservableCollection<Item>(_db.GetItems());
            ItemSelection = new SelectionModel<Item>();
            ItemSelection.SelectionChanged += SelectionChanged;

            ContainerListView = new ContainerListView();
            Containers = new ObservableCollection<Container>();
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Item> e)
        {
            var selectedItem = ItemSelection.SelectedItem;
            if (selectedItem == null)
            {
                Debug.WriteLine($"Nothing selected");
                return;
            }
            var finder = new ItemContainerFinder(_db.loot.Data);

            Containers.Clear();
            var results = finder.GetItemContainers(selectedItem.Name);
            foreach (var container in results.ContainerResults)
            {
                var itemContainer = container.Value;
                var probCalc = new ProbabilityCalculator(itemContainer);
                var prob = probCalc.CalculateProbability(102);
                Containers.Add(new Container(container.Key, prob));
            }

        }
    }
}
