using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LootViewer.Views
{
    public partial class ConfigFileSelectorView : UserControl
    {
        public ConfigFileSelectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
