using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Controls
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ProfileTabButton_ActualThemeVariantChanged(object? sender, System.EventArgs e)
        {
        }
    }
}
