using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для регистрации пользователя
    /// </summary>
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            RegisterCommand = ReactiveCommand.Create(OnRegister);
            NavigateToLoginCommand = ReactiveCommand.Create(OnNavigateToLogin);
        }

        /// <summary>
        /// Команда регистрации пользователя
        /// </summary>
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        /// <summary>
        /// Команда перехода к экрану входа
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }

        /// <summary>
        /// Обработчик регистрации пользователя
        /// </summary>
        private void OnRegister()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }

        /// <summary>
        /// Обработчик перехода к экрану входа
        /// </summary>
        private void OnNavigateToLogin()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }
    }
}
