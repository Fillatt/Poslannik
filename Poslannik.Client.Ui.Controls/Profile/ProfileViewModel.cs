using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для профиля пользователя
    /// </summary>
    public class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            LogoutCommand = ReactiveCommand.Create(OnLogout);
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToChatsCommand = ReactiveCommand.Create(OnNavigateToChats);
        }

        /// <summary>
        /// Команда выхода из системы
        /// </summary>
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда перехода к списку чатов
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToChatsCommand { get; }

        /// <summary>
        /// Обработчик выхода из системы
        /// </summary>
        private void OnLogout()
        {
            NavigationService.ClearNavigationStack();
            NavigationService.NavigateTo<LoginViewModel>();
        }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }

        /// <summary>
        /// Обработчик перехода к списку чатов
        /// </summary>
        private void OnNavigateToChats()
        {
            NavigationService.NavigateBack();
        }
    }
}
