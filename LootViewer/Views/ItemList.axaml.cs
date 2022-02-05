using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LootViewer.Views
{
    public partial class ItemList : UserControl
    {
        public ItemList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
