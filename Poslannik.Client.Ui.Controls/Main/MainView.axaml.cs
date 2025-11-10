using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Controls
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
