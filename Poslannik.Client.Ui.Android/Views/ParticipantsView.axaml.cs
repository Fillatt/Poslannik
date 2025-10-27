using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Poslannik.Client.Ui.Android.Views;

public partial class ParticipantsView : UserControl
{
    public ParticipantsView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}