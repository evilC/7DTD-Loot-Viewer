using Avalonia.Collections;
using Avalonia.Controls.Selection;
using ConfigParsers.Blocks;
using ConfigParsers.Localization;
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
        private string _settingsFile = "Settings.json";
        private Database _db;
        private LocalizationParser _lp;
        private Dictionary<string, string> _displayNames = new();
        private BlocksParser _blockParser = new();
        private Dictionary<string, HashSet<string>> _lootListContainerNames = new();

        // Config File selector
        public ConfigFileSelectorView ConfigFileSelectorView { get; set; }
        private string? _configFilePath;
        public string? ConfigFilePath
        {
            get => _configFilePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _configFilePath, value);
                SaveSettings();
                ConfigFilePathChanged();
            }
        }

        // Loot Level
        public LootLevelView LootLevelView { get; set; }
        private string? _lootLevel;
        public string? LootLevel { 
            get => _lootLevel;
            set
            {
                this.RaiseAndSetIfChanged(ref _lootLevel, value);
                GetItemLootLists();
            }
        }

        // Item Filter
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

        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private string? _itemFilterText;

        // Loot Items
        public LootItemsView LootItemsView { get; set; }
        private ObservableCollection<LootItem> _lootItems = new ObservableCollection<LootItem>();
        public DataGridCollectionView LootItems { get; set; }

        // Loot Lists
        public LootListsView LootListsView { get; set; }
        private ObservableCollection<LootList> _lootLists;
        public DataGridCollectionView LootLists { get; set; }

        // Loot Containers
        public LootContainersView LootContainersView { get; set; }
        private ObservableCollection<string> _lootContainers;
        private SortedDictionary<string, List<string>>? _containerNames = null;

        public DataGridCollectionView LootContainers { get; set; }

        public MainWindowViewModel()
        {
            LoadSettings();

            _db = new Database();
            _lp = new LocalizationParser();

            // Config file selector
            ConfigFileSelectorView = new ConfigFileSelectorView();

            // Loot Level
            LootLevelView = new LootLevelView();
            LootLevel = "1";

            // Loot Items
            ItemFilterView = new ItemFilterView();
            LootItemsView = new LootItemsView();
            LootItems = new DataGridCollectionView(_lootItems) { Filter = IsItemVisible };
            LootItems.SortDescriptions.Add(DataGridSortDescription.FromPath("DisplayName", ListSortDirection.Ascending));
            LootItems.CurrentChanged += LootItemSelectionChanged;

            // Loot Lists
            LootListsView = new LootListsView();
            _lootLists = new ObservableCollection<LootList>();
            LootLists = new DataGridCollectionView(_lootLists);
            LootLists.SortDescriptions.Add(DataGridSortDescription.FromPath("Prob", ListSortDirection.Descending));
            LootLists.CurrentChanged += LootListSelectionChanged;

            // Loot Containers
            LootContainersView = new LootContainersView();
            //_lootContainers = new ObservableCollection<LootContainer>();
            _lootContainers = new ObservableCollection<string>();
            LootContainers = new DataGridCollectionView(_lootContainers);
            LootContainers.SortDescriptions.Add(DataGridSortDescription.FromPath("Name", ListSortDirection.Ascending));

            // Trigger loading of LootItems list
            ConfigFilePathChanged();
        }

        /// <summary>
        /// Called when the Item Filter changes
        /// </summary>
        private void ItemFilterChanged()
        {
            _lootLists.Clear();
            _lootContainers.Clear();
            LootItems.Refresh();
        }

        /// <summary>
        /// Called for each LootItem to check whether it is filtered or not
        /// </summary>
        /// <param name="obj">The LootItem to check</param>
        /// <returns>True = show, False = filtered out</returns>
        public bool IsItemVisible(object obj)
        {
            if (_itemFilterText == null) return true;
            var item = (LootItem)obj;
            return _culture.CompareInfo.IndexOf(item.DisplayName, _itemFilterText, CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        /// Called when the Config File path changes
        /// </summary>
        private void ConfigFilePathChanged()
        {
            if (!string.IsNullOrWhiteSpace(_configFilePath))
            {
                // ToDo: If Config folder path was previously valid...
                // ... then the Localization data will be already present and cached to JSON ...
                // ... so it will need to be deleted and rebuilt
                // Should maybe work out some way to see if it changed (CRC check?)
                //_lp.ClearCache();
                // Have the LocalizationParser parse Localization.xml and build a list of Display names
                // NOTE: This could take a while!
                if (_lp.LocalizationFileExists(_configFilePath))
                {
                    // If file exists in new location, then reload the cache, else take no action...
                    // ... (ie while typing in the Config File Location box, do not continually reload the cache)
                    _displayNames = _lp.GetDisplayNames(_configFilePath);
                    // Get the list of container names for each LootList from the BlocksParser
                    //_blockParser = new BlocksParser();
                    _containerNames = _blockParser.GetLootLists(_configFilePath);
                }
            }
            var lootItems = _db.OpenPath(_configFilePath);
            _lootItems.Clear();
            _lootLists.Clear();
            _lootContainers.Clear();
            _lootListContainerNames = new Dictionary<string, HashSet<string>>();
            if (lootItems != null)
            {
                foreach (var lootItem in lootItems)
                {
                    var displayName = _displayNames.ContainsKey(lootItem.Key) ? _displayNames[lootItem.Key] : lootItem.Key;
                    _lootItems.Add(new LootItem(displayName, lootItem.Key));
                }
            }
            if (_containerNames != null)
            {
                foreach (var lootList in _containerNames)
                {
                    _lootListContainerNames.Add(lootList.Key, new HashSet<string>());
                    foreach (var containerName in lootList.Value)
                    {
                        var name = _displayNames.ContainsKey(containerName) ? _displayNames[containerName] : containerName;
                        _lootListContainerNames[lootList.Key].Add(name);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a new LootItem is selected in the LootItemsView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LootItemSelectionChanged(object? sender, EventArgs e)
        {
            GetItemLootLists();
        }

        /// <summary>
        /// Update the LootList
        /// Called when a new LootItem is slected in the LootItemsView, or the LootLevel changes in the LootLevelView
        /// </summary>
        private void GetItemLootLists()
        {
            int lootLevel;

            if (_db.LootData == null) return;
            if (LootItems.CurrentItem == null) return;
            if (string.IsNullOrEmpty(_lootLevel)) return;
            try { lootLevel = Convert.ToInt32(_lootLevel); }
            catch (Exception) { return; };

            var selectedItem = (LootItem)LootItems.CurrentItem;
            var finder = new ItemContainerFinder(_db.LootData.Data);
            _lootLists.Clear();
            var results = finder.GetItemContainers(selectedItem.Name);
            foreach (var container in results.ContainerResults)
            {
                var itemContainer = container.Value;
                var probCalc = new ProbabilityCalculator(itemContainer);
                var prob = probCalc.CalculateProbability(lootLevel);
                _lootLists.Add(new LootList(container.Key, Math.Round((prob * 100), 3)));
            }
            LootLists.MoveCurrentToFirst();
        }

        /// <summary>
        /// Called when a new LootList is selected in the LootListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void LootListSelectionChanged(object? sender, EventArgs e)
        {
            GetLootContainers();
        }


        /// <summary>
        /// Called when a new LootList is selected in the LootListView
        /// ? Or when LootItem changes ?
        /// ? Or when LootLevel changes ?
        /// </summary>
        private void GetLootContainers()
        {
            _lootContainers.Clear();
            if (LootLists.CurrentItem == null) return;
            var selectedLootList = ((LootList)LootLists.CurrentItem).Name;
            if (_lootListContainerNames.ContainsKey(selectedLootList))
            {
                foreach (var containerName in _lootListContainerNames[selectedLootList])
                {
                    _lootContainers.Add(containerName);
                }
                
            }
        }

        /// <summary>
        /// Load settings from the settings file
        /// </summary>
        private void LoadSettings()
        {
            if (!File.Exists(_settingsFile)) return;
            var jsonText = File.ReadAllText(_settingsFile);
            var settings = JsonSerializer.Deserialize<Settings>(jsonText);
            if (settings == null)
            {
                return;
            }
            _configFilePath = settings.ConfigFilePath;
        }

        /// <summary>
        /// Save settings to the settings file
        /// </summary>
        private void SaveSettings()
        {
            if (_configFilePath == null) return;
            var settings = new Settings() { ConfigFilePath = _configFilePath };
            var j = JsonSerializer.Serialize(settings);
            File.WriteAllText(_settingsFile, j);
        }
    }
}
