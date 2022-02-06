using Avalonia.Controls.Selection;
using LootViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.ViewModels
{
    public class ContainerListViewModel : ViewModelBase
    {
        public ObservableCollection<Container> Containers { get; }

        public ContainerListViewModel(IEnumerable<Container> containers)
        {
            Containers = new ObservableCollection<Container>(containers);
        }
    }
}
