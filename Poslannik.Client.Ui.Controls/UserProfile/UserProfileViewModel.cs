using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для профиля другого пользователя
    /// </summary>
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IAutorizationService _authorizationService;
        private Chat? _currentChat;
        private Guid? _userId;
        private string _userName = string.Empty;
        private string _userLogin = string.Empty;
        private bool _showDeleteButton;

        public UserProfileViewModel(
            IChatService chatService,
            IUserService userService,
            IAutorizationService authorizationService)
        {
            _chatService = chatService;
            _userService = userService;
            _authorizationService = authorizationService;

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            DeleteChatCommand = ReactiveCommand.Create(OnDeleteChat);
        }

        /// <summary>
        /// Текущий чат
        /// </summary>
        public Chat? CurrentChat
        {
            get => _currentChat;
            set => this.RaiseAndSetIfChanged(ref _currentChat, value);
        }

        /// <summary>
        /// ID пользователя для отображения профиля
        /// </summary>
        public Guid? UserId
        {
            get => _userId;
            set => this.RaiseAndSetIfChanged(ref _userId, value);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserLogin
        {
            get => _userLogin;
            set => this.RaiseAndSetIfChanged(ref _userLogin, value);
        }

        /// <summary>
        /// Показывать ли кнопку удаления чата
        /// </summary>
        public bool ShowDeleteButton
        {
            get => _showDeleteButton;
            set => this.RaiseAndSetIfChanged(ref _showDeleteButton, value);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда удаления чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteChatCommand { get; }

        /// <summary>
        /// Инициализация ViewModel
        /// </summary>
        public async Task InitializeAsync()
        {
            Guid targetUserId;

            // Определяем ID пользователя для отображения
            if (UserId.HasValue)
            {
                // Если задан UserId, используем его (для просмотра профиля участника группового чата)
                targetUserId = UserId.Value;
                ShowDeleteButton = false;
            }
            else if (CurrentChat != null)
            {
                // Если задан чат, определяем собеседника (для личного чата)
                var currentUserId = _authorizationService.UserId;
                if (currentUserId == null)
                    return;

                targetUserId = currentUserId == CurrentChat.User1Id
                    ? CurrentChat.User2Id.Value
                    : CurrentChat.User1Id.Value;
                ShowDeleteButton = true;
            }
            else
            {
                return;
            }

            // Загружаем информацию о пользователе
            try
            {
                var user = await _userService.GetUserByIdAsync(targetUserId);
                if (user != null)
                {
                    UserName = user.UserName ?? "Неизвестный пользователь";
                    UserLogin = user.Login ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки информации о пользователе: {ex.Message}");
                UserName = "Неизвестный пользователь";
                UserLogin = string.Empty;
            }
        }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService?.NavigateBack();
        }

        /// <summary>
        /// Обработчик удаления чата
        /// </summary>
        private async void OnDeleteChat()
        {
            if (CurrentChat == null)
                return;

            try
            {
                await _chatService.DeleteChatAsync(CurrentChat);
                NavigationService?.ClearNavigationStack();
                NavigationService?.NavigateTo<ChatsViewModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления чата: {ex.Message}");
            }
        }
    }
}
