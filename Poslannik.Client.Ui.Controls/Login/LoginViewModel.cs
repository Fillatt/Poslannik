using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Client.Services;
using System.Reactive.Linq;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для экрана входа
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthorizationService _authService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isLoading;

        public LoginViewModel(INavigationService navigationService, AuthorizationService authService)
            : base(navigationService)
        {
            _authService = authService;

            // Команда входа с проверкой на заполненность полей
            var canLogin = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                x => x.IsLoading,
                (user, pass, loading) =>
                    !string.IsNullOrWhiteSpace(user) &&
                    !string.IsNullOrWhiteSpace(pass) &&
                    !loading);

            LoginCommand = ReactiveCommand.CreateFromTask(OnLoginAsync, canLogin);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        /// <summary>
        /// Флаг загрузки
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Команда входа в систему
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        /// <summary>
        /// Обработчик входа в систему
        /// </summary>
        private async Task OnLoginAsync()
        {
            try
            {
                IsLoading = true;

                // Вызываем авторизацию через сервис
                var result = await _authService.AuthorizeAsync(Username, Password);

                if (result.IsSuccess)
                {
                    // Успешная авторизация - переходим к чатам
                    NavigationService.NavigateTo<ChatsViewModel>();
                }
                else
                {
                    // Обработка ошибки авторизации
                    await ShowError("Ошибка авторизации", "Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                await ShowError("Ошибка подключения", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ShowError(string title, string message)
        {
            // TODO: Реализовать показ ошибки (можно через диалог или Toast)
            System.Diagnostics.Debug.WriteLine($"{title}: {message}");

            // Временное решение - очищаем пароль
            Password = string.Empty;
        }
    }
}