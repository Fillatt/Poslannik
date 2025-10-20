using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}