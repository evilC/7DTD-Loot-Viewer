using Avalonia.Controls.Selection;
using ConfigParsers.Loot;
using LootViewer.Models;
using LootViewer.Services;
using LootViewer.Views;
using ReactiveUI;
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

        public ConfigFileSelectorView ConfigFileSelectorView { get; set; }
        private string? _configFilePath;
        public string? ConfigFilePath
        {
            get => _configFilePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _configFilePath, value);
                LootPathChanged();
            }
        }

        public LootLevelView LootLevelView { get; set; }
        private string? _lootLevel;
        public string? LootLevel { 
            get => _lootLevel;
            set
            {
                this.RaiseAndSetIfChanged(ref _lootLevel, value);
                GetItemContainers();
            }
        }

        public ItemListView ItemListView { get; set; }
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();
        public SelectionModel<Item> ItemSelection { get; }


        public ContainerListView ContainerListView { get; set; }
        public ObservableCollection<Container> Containers { get; }

        public MainWindowViewModel()
        {
            _db = new Database();

            ConfigFileSelectorView = new ConfigFileSelectorView();

            LootLevelView = new LootLevelView();
            LootLevel = "1";

            ItemListView = new ItemListView();
            //Items = new ObservableCollection<Item>(_db.GetItems());
            ItemSelection = new SelectionModel<Item>();
            ItemSelection.SelectionChanged += ItemSelectionChanged;

            ContainerListView = new ContainerListView();
            Containers = new ObservableCollection<Container>();

            //ConfigFilePath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml";
            ConfigFilePath = "loot.xml";
            //LootPathChanged();
        }

        private void LootPathChanged()
        {
            var items = _db.OpenPath(_configFilePath);
            Items.Clear();
            Containers.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }

        private void ItemSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Item> e)
        {
            GetItemContainers();
        }

        private void GetItemContainers()
        {
            int lootLevel;

            if (_db.LootData == null) return;
            if (ItemSelection == null || ItemSelection.SelectedItem == null) return;
            if (string.IsNullOrEmpty(_lootLevel)) return;
            try { lootLevel = Convert.ToInt32(_lootLevel); }
            catch (Exception) { return; };

            var selectedItem = ItemSelection.SelectedItem;
            var finder = new ItemContainerFinder(_db.LootData.Data);
            Containers.Clear();
            var results = finder.GetItemContainers(selectedItem.Name);
            foreach (var container in results.ContainerResults)
            {
                var itemContainer = container.Value;
                var probCalc = new ProbabilityCalculator(itemContainer);
                var prob = probCalc.CalculateProbability(lootLevel);
                Containers.Add(new Container(container.Key, prob));
            }
        }
    }
}
