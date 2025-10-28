using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views
{
    public partial class ChatsView : UserControl
    {
        public ChatsView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}