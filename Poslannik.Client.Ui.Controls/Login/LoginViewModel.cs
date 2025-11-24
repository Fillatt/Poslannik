using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для экрана входа
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            LoginCommand = ReactiveCommand.Create(OnLogin);
        }

        /// <summary>
        /// Команда входа в систему
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        /// <summary>
        /// Обработчик входа в систему
        /// </summary>
        private void OnLogin()
        {
            NavigationService.NavigateTo<ChatsViewModel>();
        }
    }
}
