using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для профиля другого пользователя
    /// </summary>
    public class UserProfileViewModel : ViewModelBase
    {
        public UserProfileViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }
    }
}
