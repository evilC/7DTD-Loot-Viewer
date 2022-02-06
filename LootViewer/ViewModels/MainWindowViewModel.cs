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
        private Database db;

        public ItemListView ItemList { get; set; }
        public ObservableCollection<Item> Items { get; }
        public SelectionModel<Item> Selection { get; }


        public ContainerListView ContainerList { get; set; }
        public ObservableCollection<Container> Containers { get; }

        public MainWindowViewModel()
        {
            db = new Database(@"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml");
            ItemList = new ItemListView();
            Items = new ObservableCollection<Item>(db.GetItems());
            Selection = new SelectionModel<Item>();
            Selection.SelectionChanged += SelectionChanged;

            ContainerList = new ContainerListView();
            Containers = new ObservableCollection<Container>();
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Item> e)
        {
            var selectedItem = Selection.SelectedItem;
            if (selectedItem == null)
            {
                Debug.WriteLine($"Nothing selected");
                return;
            }
            var rw = new ItemContainerFinder(db.loot.Data);

            Containers.Clear();
            var results = rw.GetItemContainers(selectedItem.Description);
            foreach (var container in results.ContainerResults)
            {
                var cr = container.Value;
                var probCalc = new ProbabilityCalculator(cr);
                var prob = probCalc.CalculateProbability(102);
                Containers.Add(new Container() { Name = container.Key, Prob = prob });
            }

        }
    }
}
