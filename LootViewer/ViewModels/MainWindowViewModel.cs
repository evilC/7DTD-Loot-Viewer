using Avalonia.Collections;
using Avalonia.Controls.Selection;
using ConfigParsers.Loot;
using LootViewer.Models;
using LootViewer.Services;
using LootViewer.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace LootViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Settings _settings;
        private Database _db;

        public ConfigFileSelectorView ConfigFileSelectorView { get; set; }
        private string? _configFilePath;
        public string? ConfigFilePath
        {
            get => _configFilePath;
            set
            {
                // ToDo: All kinds of ewwww going on here, need to fix
                this.RaiseAndSetIfChanged(ref _configFilePath, value);
                if (_configFilePath != null && _configFilePath != value)
                {
                    _settings.ConfigFilePath = _configFilePath;
                    _settings.Save();
                }
                
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

        public ItemFilterView ItemFilterView { get; set; }

        public string? ItemFilterText
        {
            get => _itemFilterText;
            set
            {
                this.RaiseAndSetIfChanged(ref _itemFilterText, value);
                ItemFilterChanged();
            }
        }

        private void ItemFilterChanged()
        {
            Items.Refresh();
        }

        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private string? _itemFilterText;

        public ItemListView ItemListView { get; set; }
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        public DataGridCollectionView Items { get; set; }
        public SelectionModel<Item> ItemSelection { get; }


        public ContainerListView ContainerListView { get; set; }
        public ObservableCollection<Models.Container> _containers { get; }
        public DataGridCollectionView Containers { get; set; }

        public MainWindowViewModel()
        {
            _db = new Database();
            _settings = new Settings();

            ConfigFileSelectorView = new ConfigFileSelectorView();

            LootLevelView = new LootLevelView();
            LootLevel = "1";

            ItemFilterView = new ItemFilterView();

            ItemListView = new ItemListView();
            Items = new DataGridCollectionView(_items);
            Items.Filter = IsItemVisible;
            //Items = new ObservableCollection<Item>(_db.GetItems());
            ItemSelection = new SelectionModel<Item>();
            ItemSelection.SelectionChanged += ItemSelectionChanged;
            //Items.SortDescriptions.Add(DataGridSortDescription.FromPath(property, ListSortDirection.Ascending));
            
            ContainerListView = new ContainerListView();
            _containers = new ObservableCollection<Models.Container>();
            Containers = new DataGridCollectionView(_containers);
            Containers.SortDescriptions.Add(DataGridSortDescription.FromPath("Prob", ListSortDirection.Descending));

            _settings.Load();
            ConfigFilePath = _settings.ConfigFilePath;
            //ConfigFilePath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml";
            //LootPathChanged();
        }

        public bool IsItemVisible(object obj)
        {
            if (_itemFilterText == null) return true;
            var item = (Item)obj;
            return _culture.CompareInfo.IndexOf(item.Name, _itemFilterText, CompareOptions.IgnoreCase) >= 0;
        }

        private void LootPathChanged()
        {
            var items = _db.OpenPath(_configFilePath);
            _items.Clear();
            _containers.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    _items.Add(item);
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
            //if (ItemSelection == null || ItemSelection.SelectedItem == null) return;
            if (ItemSelection == null) return;
            if (string.IsNullOrEmpty(_lootLevel)) return;
            try { lootLevel = Convert.ToInt32(_lootLevel); }
            catch (Exception) { return; };

            //var selectedItem = ItemSelection.SelectedItem;
            var selectedItem = _items[ ItemSelection.SelectedIndex];
            var finder = new ItemContainerFinder(_db.LootData.Data);
            _containers.Clear();
            var results = finder.GetItemContainers(selectedItem.Name);
            foreach (var container in results.ContainerResults)
            {
                var itemContainer = container.Value;
                var probCalc = new ProbabilityCalculator(itemContainer);
                var prob = probCalc.CalculateProbability(lootLevel);
                _containers.Add(new Models.Container(container.Key, Math.Round((prob * 100), 3)));
            }
        }
    }
}
