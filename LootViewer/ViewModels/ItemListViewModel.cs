using LootViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.ViewModels
{
    public class ItemListViewModel : ViewModelBase
    {
        public ItemListViewModel(IEnumerable<Item> items)
        {
            Items = new ObservableCollection<Item>(items);
        }

        public ObservableCollection<Item> Items { get; }
    }
}
