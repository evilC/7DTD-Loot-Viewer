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
using System.IO;
using System.Text;
using System.Text.Json;

namespace LootViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _settingsFile = "settings.json";
        private Database _db;

        public ConfigFileSelectorView ConfigFileSelectorView { get; set; }
        private string? _configFilePath;
        public string? ConfigFilePath
        {
            get => _configFilePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _configFilePath, value);
                SaveSettings();
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
        private ObservableCollection<LootItem> _items = new ObservableCollection<LootItem>();
        public DataGridCollectionView Items { get; set; }

        public ContainerListView ContainerListView { get; set; }
        public ObservableCollection<LootContainer> _containers { get; }
        public DataGridCollectionView LootLists { get; set; }

        public MainWindowViewModel()
        {
            LoadSettings();

            _db = new Database();

            ConfigFileSelectorView = new ConfigFileSelectorView();

            LootLevelView = new LootLevelView();
            LootLevel = "1";

            ItemFilterView = new ItemFilterView();

            ItemListView = new ItemListView();
            Items = new DataGridCollectionView(_items) { Filter = IsItemVisible };
            Items.CurrentChanged += ItemSelectionChanged;

            ContainerListView = new ContainerListView();
            _containers = new ObservableCollection<LootContainer>();
            LootLists = new DataGridCollectionView(_containers);
            LootLists.SortDescriptions.Add(DataGridSortDescription.FromPath("Prob", ListSortDirection.Descending));

            LootPathChanged();
        }

        public bool IsItemVisible(object obj)
        {
            if (_itemFilterText == null) return true;
            var item = (LootItem)obj;
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

        private void ItemSelectionChanged(object? sender, EventArgs e)
        {
            GetItemContainers();
        }

        private void GetItemContainers()
        {
            int lootLevel;

            if (_db.LootData == null) return;
            if (Items.CurrentItem == null) return;
            if (string.IsNullOrEmpty(_lootLevel)) return;
            try { lootLevel = Convert.ToInt32(_lootLevel); }
            catch (Exception) { return; };

            var selectedItem = (LootItem)Items.CurrentItem;
            var finder = new ItemContainerFinder(_db.LootData.Data);
            _containers.Clear();
            var results = finder.GetItemContainers(selectedItem.Name);
            foreach (var container in results.ContainerResults)
            {
                var itemContainer = container.Value;
                var probCalc = new ProbabilityCalculator(itemContainer);
                var prob = probCalc.CalculateProbability(lootLevel);
                _containers.Add(new LootContainer(container.Key, Math.Round((prob * 100), 3)));
            }
        }

        private void LoadSettings()
        {
            if (!File.Exists(_settingsFile)) return;
            using (var fs = File.OpenRead(_settingsFile))
            {
                Settings? s = LoadFromStream(fs);
                if (s == null)
                {
                    return;
                }
                _configFilePath = s.ConfigFilePath;
            }
        }

        private Settings? LoadFromStream(FileStream stream)
        {
            return JsonSerializer.Deserialize<Settings>(stream);
        }

        private void SaveSettings()
        {
            if (_configFilePath == null) return;
            var settings = new Settings() { ConfigFilePath = _configFilePath };
            using (var fs = File.OpenWrite(_settingsFile))
            {
                SaveToStream(settings, fs);
            }
        }

        private static void SaveToStream(Settings data, Stream stream)
        {
            JsonSerializer.Serialize(stream, data);
        }
    }
}
