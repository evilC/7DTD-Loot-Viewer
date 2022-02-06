using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LootViewer.Views
{
    public partial class ContainerListView : UserControl
    {
        public ContainerListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
