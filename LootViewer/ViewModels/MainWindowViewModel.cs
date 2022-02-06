using LootViewer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LootViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(Database db)
        {
            ItemList = new ItemListViewModel(db.GetItems());
            ContainerList = new ContainerListViewModel(db.GetContainers());
        }

        public ItemListViewModel ItemList { get; }
        public ContainerListViewModel ContainerList { get; }
    }
}
