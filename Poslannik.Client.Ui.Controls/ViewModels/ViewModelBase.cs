using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Controls.ViewModels
{
    /// <summary>
    /// Базовый класс для всех ViewModel
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject
    {
        /// <summary>Сервис навигации.</summary>
        public INavigationService NavigationService { get; set; }
    }
}
