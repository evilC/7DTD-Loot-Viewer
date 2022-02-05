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
            List = new ItemListViewModel(db.GetItems());
        }

        public ItemListViewModel List { get; }
    }
}
