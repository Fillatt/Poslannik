using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Controls
{
    public partial class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            AvaloniaXamlLoader.Load(this);

            this.DataContextChanged += async (sender, args) =>
            {
                if (DataContext is UserProfileViewModel viewModel)
                {
                    await viewModel.InitializeAsync();
                }
            };
        }
    }
}
