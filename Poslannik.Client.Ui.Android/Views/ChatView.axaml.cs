using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}