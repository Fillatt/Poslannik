using Poslannik.Client.Services;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для списка чатов
    /// </summary>
    public class ChatsViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly ChatViewModel _chatViewModel;
        private readonly IAutorizationService _autorizationService;
        private readonly IUserService _userService;

        private ObservableCollection<Chat> _chats;
        private bool _isLoading;

        public ChatsViewModel(
            IChatService chatService,
            IUserService userService,
            IAutorizationService autorizationService,
            ChatViewModel chatViewModel)
        {
            _chatService = chatService;
            _chatViewModel = chatViewModel;
            _autorizationService = autorizationService;
            _userService = userService;

            _chats = new ObservableCollection<Chat>();

            NavigateToProfileCommand = ReactiveCommand.Create(OnNavigateToProfile);
            NavigateToChatCommand = ReactiveCommand.Create<Chat, Task>(OnNavigateToChatAsync);
            NavigateToNewChatCommand = ReactiveCommand.Create(OnNavigateToNewChat);

            SubscribeToChatEvents();
        }

        /// <summary>
        /// Коллекция чатов
        /// </summary>
        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set => this.RaiseAndSetIfChanged(ref _chats, value);
        }

        /// <summary>
        /// Флаг загрузки данных
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Команда перехода к профилю
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToProfileCommand { get; }

        /// <summary>
        /// Команда перехода к чату
        /// </summary>
        public ReactiveCommand<Chat, Task> NavigateToChatCommand { get; }

        /// <summary>
        /// Команда перехода к созданию нового чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToNewChatCommand { get; }

        /// <summary>
        /// Обработчик перехода к профилю
        /// </summary>
        private void OnNavigateToProfile()
        {
            NavigationService.NavigateToWithHistory<ProfileViewModel>();
        }

        /// <summary>
        /// Обработчик перехода к чату
        /// </summary>
        private async Task OnNavigateToChatAsync(Chat chat)
        {
            _chatViewModel.CurrentChat = chat;
            await _chatViewModel.InitializeAsync();
            NavigationService.NavigateToWithHistory<ChatViewModel>();
        }

        /// <summary>
        /// Обработчик перехода к созданию нового чата
        /// </summary>
        private void OnNavigateToNewChat()
        {
            NavigationService.NavigateToWithHistory<NewChatViewModel>();
        }

        /// <summary>
        /// Инициализация и загрузка чатов
        /// </summary>
        public async Task InitializeAsync()
        {
            // Загружаем чаты
            await LoadChatsAsync();
        }

        /// <summary>
        /// Загрузка списка чатов пользователя
        /// </summary>
        private async Task LoadChatsAsync()
        {
            IsLoading = true;

            try
            {
                var chats = await _chatService.GetUserChatsAsync();
                var privateChats = chats.Where(x => x.ChatType == ChatType.Private);
                foreach (var chat in privateChats)
                {
                    var user = _autorizationService.UserId == chat.User1Id ? await _userService.GetUserByIdAsync(chat.User2Id.Value) : await _userService.GetUserByIdAsync(chat.User1Id.Value);
                    chat.Name = user.UserName;
                }

                Chats.Clear();
                foreach (var chat in chats)
                {
                    Chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки чатов: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Подписка на события ChatService
        /// </summary>
        private void SubscribeToChatEvents()
        {
            _chatService.OnChatCreated += OnChatCreated;
            _chatService.OnChatUpdated += OnChatUpdated;
            _chatService.OnChatDeleted += OnChatDeleted;
            _chatService.OnParticipantRemoved += OnParticipantRemoved;
        }

        /// <summary>
        /// Обработчик создания нового чата
        /// </summary>
        private async void OnChatCreated(Chat chat)
        {
            await LoadChatsAsync();
        }

        /// <summary>
        /// Обработчик обновления чата
        /// </summary>
        private async void OnChatUpdated(Chat chat)
        {
            await LoadChatsAsync();
        }

        /// <summary>
        /// Обработчик удаления чата
        /// </summary>
        private void OnChatDeleted(Guid chatId)
        {
            // Удаляем чат из списка
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                Chats.Remove(chat);
            }
        }

        /// <summary>
        /// Обработчик удаления участника из чата
        /// </summary>
        private void OnParticipantRemoved(Guid chatId, Guid userId)
        {
            // Если удалили текущего пользователя, удаляем чат из списка
            if (userId == _autorizationService.UserId)
            {
                var chat = Chats.FirstOrDefault(c => c.Id == chatId);
                if (chat != null)
                {
                    Chats.Remove(chat);
                }
            }
        }
    }
}
