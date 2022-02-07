using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LootViewer.Views
{
    public partial class LootContainersView : UserControl
    {
        public LootContainersView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
