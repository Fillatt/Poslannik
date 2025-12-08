using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using Poslannik.Client.Services.Interfaces;
using System.Linq;
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


        private ObservableCollection<Chat> _chats;
        private bool _isLoading;

        public ChatsViewModel(
            IChatService chatService,
            ChatViewModel chatViewModel)
        {
            _chatService = chatService;
            _chatViewModel = chatViewModel;

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
        }

        /// <summary>
        /// Обработчик создания нового чата
        /// </summary>
        private void OnChatCreated(Chat chat)
        {
            // Добавляем новый чат в список, если его там нет
            if (!Chats.Any(c => c.Id == chat.Id))
            {
                Chats.Add(chat);
            }
        }

        /// <summary>
        /// Обработчик обновления чата
        /// </summary>
        private void OnChatUpdated(Chat chat)
        {
            // Находим и обновляем чат в списке
            var existingChat = Chats.FirstOrDefault(c => c.Id == chat.Id);
            if (existingChat != null)
            {
                var index = Chats.IndexOf(existingChat);
                Chats[index] = chat;
            }
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
    }
}
