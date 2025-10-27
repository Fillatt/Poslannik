using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views;

public partial class UserProfileView : UserControl
{
    public UserProfileView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}