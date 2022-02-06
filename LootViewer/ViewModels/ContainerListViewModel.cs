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
        public SelectionModel<Container> Selection { get; }

        public ContainerListViewModel(IEnumerable<Container> containers)
        {
            Containers = new ObservableCollection<Container>(containers);
            Selection = new SelectionModel<Container>();
            Selection.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Container> e)
        {
            throw new NotImplementedException();
        }
    }
}
