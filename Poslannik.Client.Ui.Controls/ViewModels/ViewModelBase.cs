using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Controls.ViewModels
{
    /// <summary>
    /// Базовый класс для всех ViewModel
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject
    {
        protected INavigationService NavigationService { get; }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
