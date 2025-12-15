using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Controls
{
    public partial class ParticipantsView : UserControl
    {
        public ParticipantsView()
        {
            AvaloniaXamlLoader.Load(this);

            this.DataContextChanged += async (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine($"ParticipantsView.DataContextChanged: DataContext type = {DataContext?.GetType().Name}");

                if (DataContext is ParticipantsViewModel viewModel)
                {
                    System.Diagnostics.Debug.WriteLine($"ParticipantsView.DataContextChanged: viewModel.CurrentChat = {viewModel.CurrentChat?.Id}");

                    if (viewModel.CurrentChat != null)
                    {
                        System.Diagnostics.Debug.WriteLine("ParticipantsView.DataContextChanged: Calling InitializeAsync");
                        await viewModel.InitializeAsync();
                        System.Diagnostics.Debug.WriteLine("ParticipantsView.DataContextChanged: InitializeAsync completed");
                    }
                }
            };
        }
    }
}
