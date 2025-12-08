using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Client.Services.Interfaces;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для профиля пользователя
    /// </summary>
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAutorizationService _autorizationService;
        private string? _userName;
        private string? _displayName;

        public ProfileViewModel(IUserService userService, IAutorizationService autorizationService)
        {
            _userService = userService;
            _autorizationService = autorizationService;

            LogoutCommand = ReactiveCommand.Create(OnLogout);
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToChatsCommand = ReactiveCommand.Create(OnNavigateToChats);
        }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string? UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        /// <summary>
        /// Отображаемое имя пользователя
        /// </summary>
        public string? DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        /// <summary>
        /// Инициализация и загрузка данных профиля
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadUserDataAsync();
        }

        /// <summary>
        /// Загрузка данных пользователя
        /// </summary>
        private async Task LoadUserDataAsync()
        {
            if (_autorizationService.UserId == null)
                return;

            try
            {
                var user = await _userService.GetUserByIdAsync(_autorizationService.UserId.Value);
                if (user != null)
                {
                    UserName = user.UserName;
                    DisplayName = user.UserName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки данных пользователя: {ex.Message}");
            }
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
