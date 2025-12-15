using Poslannik.Client.Services;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для личного чата
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        private Chat? _currentChat;
        private UserProfileViewModel _userProfileViewModel;
        private string _messageText = string.Empty;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IAutorizationService _authorizationService;
        private readonly IChatService _chatService;
        private readonly ParticipantsViewModel _participantsViewModel;
        private readonly Dictionary<Guid, string> _userNamesCache = new();
        private IReadOnlyList<Message> _messages;
        private string? _chatName;
        private bool _isGroupChat;

        public ChatViewModel(
            IMessageService messageService,
            IUserService userService,
            IChatService chatService,
            IAutorizationService authorizationService,
            ParticipantsViewModel participantsViewModel,
            UserProfileViewModel userProfileViewModel)
            
        {

            _messageService = messageService;
            _userService = userService;
            _authorizationService = authorizationService;
            _participantsViewModel = participantsViewModel;
            _chatService = chatService;
            _userProfileViewModel = userProfileViewModel;

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToUserProfileCommand = ReactiveCommand.Create(OnNavigateToUserProfileAsync);
            NavigateToParticipantsCommand = ReactiveCommand.Create(OnNavigateToParticipants);
            SendMessageCommand = ReactiveCommand.Create(OnSendMessageAsync);

            // Подписываемся на событие получения нового сообщения
            _messageService.OnMessageSended += OnMessageReceived;
            _chatService.OnChatDeleted += OnChatDeleted;
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
            }
        }

        public bool IsGroupChat
        {
            get => _isGroupChat;
            set => this.RaiseAndSetIfChanged(ref _isGroupChat, value);
        }

        public string? ChatName
        {
            get => _chatName;
            set => this.RaiseAndSetIfChanged(ref _chatName, value);
        }

        /// <summary>
        /// Коллекция сообщений для отображения
        /// </summary>
        public ObservableCollection<MessageViewModel> Messages { get; } = new();

        /// <summary>
        /// Текст вводимого сообщения
        /// </summary>
        public string MessageText
        {
            get => _messageText;
            set => this.RaiseAndSetIfChanged(ref _messageText, value);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда перехода к профилю пользователя
        /// </summary>
        public ReactiveCommand<Unit, Task> NavigateToUserProfileCommand { get; }

        /// <summary>
        /// Команда перехода к списку участников
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToParticipantsCommand { get; }

        /// <summary>
        /// Команда отправки сообщения
        /// </summary>
        public ReactiveCommand<Unit, Task> SendMessageCommand { get; }

        public async Task InitializeAsync()
        {
            MessageText = string.Empty;
            _messages = await _messageService.GetAllMessagesByChatId(_currentChat.Id);
            if (_currentChat?.ChatType == ChatType.Private)
            {
                IsGroupChat = false;
                var userId = _authorizationService.UserId == _currentChat.User1Id ? _currentChat.User2Id.Value : _currentChat.User1Id.Value;
                ChatName = await GetUserName(userId);
            }
            else
            {
                IsGroupChat = true;
                ChatName = _currentChat?.Name;
            }
            await ReloadMessagesAsync(_messages);
        }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService?.NavigateBack();
        }

        /// <summary>
        /// Обработчик перехода к профилю пользователя
        /// </summary>
        private async Task OnNavigateToUserProfileAsync()
        {
            if (CurrentChat != null && !IsGroupChat)
            {
                _userProfileViewModel.CurrentChat = CurrentChat;
                _userProfileViewModel.UserId = null;
                await _userProfileViewModel.InitializeAsync();
                NavigationService?.NavigateToWithHistory<UserProfileViewModel>();
            }
        }

        /// <summary>
        /// Обработчик перехода к списку участников
        /// </summary>
        private void OnNavigateToParticipants()
        {
            if (CurrentChat != null && IsGroupChat)
            {
                _participantsViewModel.CurrentChat = CurrentChat;
                NavigationService?.NavigateToWithHistory<ParticipantsViewModel>();
            }
        }

        /// <summary>
        /// Обработчик отправки сообщения
        /// </summary>
        private async Task OnSendMessageAsync()
        {
            // Проверяем, что есть текст и чат выбран
            if (string.IsNullOrWhiteSpace(MessageText) || CurrentChat == null)
                return;

            var currentUserId = _authorizationService.UserId;
            if (currentUserId == null)
                return;

            // Создаем новое сообщение
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ChatId = CurrentChat.Id,
                SenderId = currentUserId.Value,
                Data = MessageText.Trim(),
                DateTime = DateTime.UtcNow
            };

            // Отправляем сообщение через сервис
            try
            {
                await _messageService.SendMessageAsync(message);

                // Очищаем поле ввода после успешной отправки
                MessageText = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик получения нового сообщения
        /// </summary>
        private async void OnMessageReceived(Message message)
        {
            // Проверяем, что сообщение относится к текущему чату
            if (CurrentChat == null || message.ChatId != CurrentChat.Id)
                return;

            await AddMessageToList(message);
        }

        private async Task ReloadMessagesAsync(IReadOnlyList<Message> messages)
        {
            Messages.Clear();
            foreach (var message in messages) await AddMessageToList(message);
        }

        /// <summary>
        /// Добавляет сообщение в список с сортировкой по времени
        /// </summary>
        private async Task AddMessageToList(Message message)
        {
            var currentUserId = _authorizationService.UserId;
            if (currentUserId == null)
                return;

            var isOwnMessage = message.SenderId == currentUserId.Value;
            string? senderName = null;

            // Получаем имя отправителя, если это не собственное сообщение
            if (!isOwnMessage)
            {
                senderName = await GetUserName(message.SenderId);
            }

            var messageViewModel = new MessageViewModel
            {
                MessageId = message.Id,
                SenderId = message.SenderId,
                Text = message.Data,
                DateTime = message.DateTime,
                IsOwnMessage = isOwnMessage,
                SenderName = senderName
            };

            // Вставляем сообщение в правильную позицию (сортировка по времени)
            int insertIndex = 0;
            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].DateTime <= message.DateTime)
                {
                    insertIndex = i + 1;
                }
                else
                {
                    break;
                }
            }

            Messages.Insert(insertIndex, messageViewModel);
        }

        /// <summary>
        /// Получает имя пользователя (с кешированием)
        /// </summary>
        private async Task<string> GetUserName(Guid userId)
        {
            // Проверяем кеш
            if (_userNamesCache.TryGetValue(userId, out var cachedName))
            {
                return cachedName;
            }

            // Загружаем из сервиса
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                var userName = user?.UserName ?? user?.Login ?? "Неизвестный пользователь";
                _userNamesCache[userId] = userName;
                return userName;
            }
            catch
            {
                return "Неизвестный пользователь";
            }
        }

        private async void OnChatDeleted(Guid chatId)
        {
            if(chatId == CurrentChat?.Id)
            {
                NavigationService?.ClearNavigationStack();
                NavigationService?.NavigateTo<ChatsViewModel>();
                return;
            }
        }
    }
}
