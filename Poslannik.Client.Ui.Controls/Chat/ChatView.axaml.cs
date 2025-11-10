using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Controls
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
