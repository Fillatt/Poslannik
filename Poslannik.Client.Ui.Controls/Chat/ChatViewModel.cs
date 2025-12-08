using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using Poslannik.Client.Services.Interfaces;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для личного чата
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        private Chat? _currentChat;
        private string? _messageText;
        private readonly IMessageService _messageService;
        private readonly IAutorizationService _autorizationService;
        private readonly IUserService _userService;
        private readonly UserProfileViewModel _userProfileViewModel;
        private string? _chatName;

        public ChatViewModel(
            IMessageService messageService,
            IAutorizationService autorizationService,
            IUserService userService,
            UserProfileViewModel userProfileViewModel)
        {
            _messageService = messageService;
            _autorizationService = autorizationService;
            _userService = userService;
            _userProfileViewModel = userProfileViewModel;

            Messages = new ObservableCollection<MessageViewModel>();

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToUserProfileCommand = ReactiveCommand.Create(OnNavigateToUserProfile);
            SendMessageCommand = ReactiveCommand.Create(OnSendMessage);

            // Подписываемся на события получения сообщений
            _messageService.OnMessageReceived += OnMessageReceived;
            _messageService.OnMessageSent += OnMessageSent;
        }

        /// <summary>
        /// Текущий чат
        /// </summary>
        public Chat? CurrentChat
        {
            get => _currentChat;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentChat, value);
                if (value != null)
                {
                    _ = LoadMessagesAsync();
                    _ = LoadChatNameAsync();
                }
            }
        }

        /// <summary>
        /// Название чата (имя собеседника)
        /// </summary>
        public string? ChatName
        {
            get => _chatName;
            set => this.RaiseAndSetIfChanged(ref _chatName, value);
        }

        /// <summary>
        /// Текст нового сообщения
        /// </summary>
        public string? MessageText
        {
            get => _messageText;
            set => this.RaiseAndSetIfChanged(ref _messageText, value);
        }

        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        public ObservableCollection<MessageViewModel> Messages { get; }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда перехода к профилю пользователя
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToUserProfileCommand { get; }

        /// <summary>
        /// Команда отправки сообщения
        /// </summary>
        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }

        /// <summary>
        /// Загрузка сообщений чата
        /// </summary>
        private async Task LoadMessagesAsync()
        {
            if (CurrentChat == null || _autorizationService.UserId == null)
                return;

            try
            {
                var messages = await _messageService.GetChatMessagesAsync(CurrentChat.Id);

                Messages.Clear();
                foreach (var message in messages)
                {
                    Messages.Add(new MessageViewModel(message, _autorizationService.UserId.Value));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки сообщений: {ex.Message}");
            }
        }

        /// <summary>
        /// Загрузка имени чата (имени собеседника)
        /// </summary>
        private async Task LoadChatNameAsync()
        {
            if (CurrentChat == null || _autorizationService.UserId == null)
                return;

            try
            {
                if (CurrentChat.ChatType == ChatType.Private)
                {
                    // Определяем ID собеседника
                    var otherUserId = CurrentChat.User1Id == _autorizationService.UserId
                        ? CurrentChat.User2Id
                        : CurrentChat.User1Id;

                    if (otherUserId.HasValue)
                    {
                        var otherUser = await _userService.GetUserByIdAsync(otherUserId.Value);
                        ChatName = otherUser?.DisplayName ?? "Пользователь";
                    }
                }
                else
                {
                    ChatName = CurrentChat.Name ?? "Групповой чат";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки имени чата: {ex.Message}");
                ChatName = "Чат";
            }
        }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }

        /// <summary>
        /// Обработчик перехода к профилю пользователя
        /// </summary>
        private void OnNavigateToUserProfile()
        {
            if (CurrentChat != null && _autorizationService.UserId != null)
            {
                // Определяем ID собеседника
                var otherUserId = CurrentChat.User1Id == _autorizationService.UserId
                    ? CurrentChat.User2Id
                    : CurrentChat.User1Id;

                if (otherUserId.HasValue)
                {
                    _userProfileViewModel.UserId = otherUserId.Value;
                    NavigationService.NavigateToWithHistory<UserProfileViewModel>();
                }
            }
        }

        /// <summary>
        /// Обработчик отправки сообщения
        /// </summary>
        private async void OnSendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText) || CurrentChat == null)
                return;

            try
            {
                var message = await _messageService.SendMessageAsync(CurrentChat.Id, MessageText);

                if (message != null)
                {
                    MessageText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик получения нового сообщения
        /// </summary>
        private void OnMessageReceived(Message message)
        {
            if (message.ChatId == CurrentChat?.Id && _autorizationService.UserId != null)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    Messages.Add(new MessageViewModel(message, _autorizationService.UserId.Value));
                });
            }
        }

        /// <summary>
        /// Обработчик подтверждения отправки сообщения
        /// </summary>
        private void OnMessageSent(Message message)
        {
            if (message.ChatId == CurrentChat?.Id && _autorizationService.UserId != null)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    // Проверяем, не добавлено ли уже сообщение
                    if (!Messages.Any(m => m.Id == message.Id))
                    {
                        Messages.Add(new MessageViewModel(message, _autorizationService.UserId.Value));
                    }
                });
            }
        }
    }
}
