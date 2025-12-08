using Avalonia.Controls;
using Avalonia.Input;
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
    /// ViewModel для создания нового чата
    /// </summary>
    public class NewChatViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly IAutorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly ChatViewModel _chatViewModel;
        private readonly GroupChatViewModel _groupChatViewModel;

        private bool _isPrivateChat;
        private string? _chatName;
        private bool _isLoading;
        private string? _errorMessage;

        private string? _userSearchQuery;
        private string? _participantSearchQuery;
        private User? _selectedUser;

        private ObservableCollection<User> _foundUsers;
        private ObservableCollection<User> _foundParticipants;
        private ObservableCollection<User> _participants;

        public NewChatViewModel(IChatService chatService, IAutorizationService authorizationService, IUserService userService, ChatViewModel chatViewModel, GroupChatViewModel groupChatViewModel)
        {
            _chatService = chatService;
            _authorizationService = authorizationService;
            _userService = userService;
            _chatViewModel = chatViewModel;
            _groupChatViewModel = groupChatViewModel;

            _foundUsers = new ObservableCollection<User>();
            _foundParticipants = new ObservableCollection<User>();
            _participants = new ObservableCollection<User>();

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            CreateChatCommand = ReactiveCommand.Create(OnCreateChatAsync);
            RemoveParticipantCommand = ReactiveCommand.Create<User>(OnRemoveParticipant);
            SelectUserCommand = ReactiveCommand.Create<User>(OnSelectUser);
            AddParticipantCommand = ReactiveCommand.Create<User>(OnAddParticipant);
            KeyDownCommand = ReactiveCommand.Create<string?, Task>(SearchUsersAsync);
            SearchParticipantsCommand = ReactiveCommand.Create<string?, Task>(SearchParticipantsAsync);
        }

        /// <summary>
        /// Флаг личного чата
        /// </summary>
        public bool IsPrivateChat
        {
            get => _isPrivateChat;
            set => this.RaiseAndSetIfChanged(ref _isPrivateChat, value);
        }

        /// <summary>
        /// Название чата
        /// </summary>
        public string? ChatName
        {
            get => _chatName;
            set => this.RaiseAndSetIfChanged(ref _chatName, value);
        }

        /// <summary>
        /// Флаг процесса загрузки
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        /// <summary>
        /// Поисковый запрос для личного чата
        /// </summary>
        public string? UserSearchQuery
        {
            get => _userSearchQuery;
            set => this.RaiseAndSetIfChanged(ref _userSearchQuery, value);
        }

        /// <summary>
        /// Поисковый запрос для участников группы
        /// </summary>
        public string? ParticipantSearchQuery
        {
            get => _participantSearchQuery;
            set => this.RaiseAndSetIfChanged(ref _participantSearchQuery, value);
        }

        /// <summary>
        /// Выбранный пользователь для личного чата
        /// </summary>
        public User? SelectedUser
        {
            get => _selectedUser;
            set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
        }

        /// <summary>
        /// Найденные пользователи для личного чата
        /// </summary>
        public ObservableCollection<User> FoundUsers
        {
            get => _foundUsers;
            set => this.RaiseAndSetIfChanged(ref _foundUsers, value);
        }

        /// <summary>
        /// Найденные участники для группового чата
        /// </summary>
        public ObservableCollection<User> FoundParticipants
        {
            get => _foundParticipants;
            set => this.RaiseAndSetIfChanged(ref _foundParticipants, value);
        }

        /// <summary>
        /// Список участников группового чата
        /// </summary>
        public ObservableCollection<User> Participants
        {
            get => _participants;
            set => this.RaiseAndSetIfChanged(ref _participants, value);
        }

        /// <summary>
        /// Проверка наличия участников
        /// </summary>
        public bool HasParticipants => _participants.Count > 0;

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда создания чата
        /// </summary>
        public ReactiveCommand<Unit, Task> CreateChatCommand { get; }

        /// <summary>
        /// Команда удаления участника
        /// </summary>
        public ReactiveCommand<User, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Команда выбора пользователя для личного чата
        /// </summary>
        public ReactiveCommand<User, Unit> SelectUserCommand { get; }

        /// <summary>
        /// Команда добавления участника в групповой чат
        /// </summary>
        public ReactiveCommand<User, Unit> AddParticipantCommand { get; }

        public ReactiveCommand<string?, Task> KeyDownCommand { get; }

        /// <summary>
        /// Команда поиска участников для группового чата
        /// </summary>
        public ReactiveCommand<string?, Task> SearchParticipantsCommand { get; }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            ResetState();
            NavigationService.NavigateBack();
        }

        /// <summary>
        /// Сброс состояния формы
        /// </summary>
        private void ResetState()
        {
            IsPrivateChat = true;
            ChatName = null;
            UserSearchQuery = null;
            ParticipantSearchQuery = null;
            SelectedUser = null;
            ErrorMessage = null;
            FoundUsers.Clear();
            FoundParticipants.Clear();
            Participants.Clear();
            this.RaisePropertyChanged(nameof(HasParticipants));
        }

        /// <summary>
        /// Поиск пользователей для личного чата
        /// </summary>
        private async Task SearchUsersAsync(string? userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                FoundUsers.Clear();
                return;
            }

            try
            {
                var users = await _userService.SearchUsersAsync(userName);
                var existingChats = await _chatService.GetUserChatsAsync();

                FoundUsers.Clear();
                foreach (var user in users.Where(u => u.Id != _authorizationService.UserId))
                {
                    // Проверяем, нет ли уже личного чата с этим пользователем
                    var chatExists = existingChats.Any(c =>
                        c.ChatType == ChatType.Private &&
                        ((c.User1Id == _authorizationService.UserId && c.User2Id == user.Id) ||
                         (c.User2Id == _authorizationService.UserId && c.User1Id == user.Id)));

                    if (!chatExists)
                    {
                        FoundUsers.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка поиска пользователей: {ex.Message}");
            }
        }

        /// <summary>
        /// Поиск участников для группового чата
        /// </summary>
        private async Task SearchParticipantsAsync(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FoundParticipants.Clear();
                return;
            }

            try
            {
                var users = await _userService.SearchUsersAsync(query);
                FoundParticipants.Clear();
                foreach (var user in users.Where(u => u.Id != _authorizationService.UserId && !_participants.Any(p => p.Id == u.Id)))
                {
                    FoundParticipants.Add(user);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка поиска участников: {ex.Message}");
            }
        }

        /// <summary>
        /// Выбор пользователя для личного чата
        /// </summary>
        private void OnSelectUser(User user)
        {
            SelectedUser = user;
            FoundUsers.Clear();
            UserSearchQuery = string.Empty;
        }

        /// <summary>
        /// Добавление участника в групповой чат
        /// </summary>
        private void OnAddParticipant(User user)
        {
            if (!Participants.Any(p => p.Id == user.Id))
            {
                Participants.Add(user);
                this.RaisePropertyChanged(nameof(HasParticipants));
            }
            FoundParticipants.Clear();
            ParticipantSearchQuery = string.Empty;
        }

        /// <summary>
        /// Обработчик создания чата
        /// </summary>
        private async Task OnCreateChatAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            ErrorMessage = null;

            try
            {
                if (_authorizationService.UserId == null)
                {
                    ErrorMessage = "Пользователь не авторизован";
                    return;
                }

                Chat newChat;

                if (IsPrivateChat)
                {
                    // Создание личного чата
                    if (SelectedUser == null)
                    {
                        ErrorMessage = "Выберите пользователя";
                        return;
                    }

                    newChat = new Chat
                    {
                        Id = Guid.NewGuid(),
                        ChatType = ChatType.Private,
                        User1Id = _authorizationService.UserId.Value,
                        User2Id = SelectedUser.Id,
                        Name = SelectedUser.UserName,
                        EncryptedGroupKey = null,
                        AdminId = null
                    };
                }
                else
                {
                    // Создание группового чата
                    if (string.IsNullOrWhiteSpace(ChatName))
                    {
                        ErrorMessage = "Введите название чата";
                        return;
                    }

                    if (Participants.Count == 0)
                    {
                        ErrorMessage = "Добавьте хотя бы одного участника";
                        return;
                    }

                    // Проверяем, нет ли уже группового чата с таким же названием
                    var existingChats = await _chatService.GetUserChatsAsync();
                    var groupChatExists = existingChats.Any(c =>
                        c.ChatType == ChatType.Group &&
                        c.Name != null &&
                        c.Name.Trim().Equals(ChatName.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (groupChatExists)
                    {
                        ErrorMessage = "Групповой чат с таким названием уже существует";
                        return;
                    }

                    // TODO: Генерация EncryptedGroupKey через Signal-протокол
                    // Пока используем временную заглушку
                    var encryptedGroupKey = new byte[] { 0x00 };

                    newChat = new Chat
                    {
                        Id = Guid.NewGuid(),
                        ChatType = ChatType.Group,
                        Name = ChatName,
                        EncryptedGroupKey = encryptedGroupKey,
                        AdminId = _authorizationService.UserId.Value,
                        User1Id = null,
                        User2Id = null
                    };
                }

                Chat? createdChat;

                // Передаем участников при создании группового чата
                if (!IsPrivateChat && Participants.Count > 0)
                {
                    var participantIds = Participants.Select(p => p.Id).ToList();
                    createdChat = await _chatService.CreateChatAsync(newChat, participantIds);
                }
                else
                {
                    createdChat = await _chatService.CreateChatAsync(newChat);
                }

                if (createdChat != null)
                {
                    // Сбрасываем состояние формы
                    ResetState();

                    // Очищаем стек и добавляем ChatsViewModel в основу
                    NavigationService.ClearNavigationStack();
                    NavigationService.NavigateToWithHistory<ChatsViewModel>();

                    // Передаем созданный чат в соответствующий ViewModel и переходим к нему
                    if (IsPrivateChat)
                    {
                        _chatViewModel.CurrentChat = createdChat;
                        NavigationService.NavigateToWithHistory<ChatViewModel>();
                    }
                    else
                    {
                        _groupChatViewModel.CurrentChat = createdChat;
                        NavigationService.NavigateToWithHistory<GroupChatViewModel>();
                    }
                }
                else
                {
                    ErrorMessage = "Не удалось создать чат";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при создании чата";
                System.Diagnostics.Debug.WriteLine($"Ошибка создания чата: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Обработчик удаления участника
        /// </summary>
        private void OnRemoveParticipant(User user)
        {
            Participants.Remove(user);
            this.RaisePropertyChanged(nameof(HasParticipants));
        }
    }
}
