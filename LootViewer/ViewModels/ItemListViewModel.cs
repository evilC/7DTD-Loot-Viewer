using Avalonia.Controls.Selection;
using LootViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.ViewModels
{
    public class ItemListViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; }
        public SelectionModel<Item> Selection { get; }

        public ItemListViewModel(IEnumerable<Item> items)
        {
            Items = new ObservableCollection<Item>(items);
            Selection = new SelectionModel<Item>();
            Selection.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Item> e)
        {
            var foo = Selection.SelectedItem;
            if (foo == null)
            {
                Debug.WriteLine($"Nothing selected");
                return;
            }
            Debug.WriteLine($"Selected {foo.Description}");
        }
    }
}
