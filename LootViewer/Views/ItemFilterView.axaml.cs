using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LootViewer.Views
{
    public partial class ItemFilterView : UserControl
    {
        public ItemFilterView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
