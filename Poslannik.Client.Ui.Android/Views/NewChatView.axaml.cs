using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views;

public partial class NewChatView : UserControl
{
    public NewChatView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}