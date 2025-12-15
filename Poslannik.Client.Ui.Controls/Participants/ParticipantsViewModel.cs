using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для списка участников группового чата
    /// </summary>
    public class ParticipantsViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IAutorizationService _authorizationService;
        private readonly UserProfileViewModel _userProfileViewModel;
        private Chat? _currentChat;
        private bool _isAdmin;

        public ParticipantsViewModel(
            IChatService chatService,
            IUserService userService,
            IAutorizationService authorizationService,
            UserProfileViewModel userProfileViewModel)
        {
            _chatService = chatService;
            _userService = userService;
            _authorizationService = authorizationService;
            _userProfileViewModel = userProfileViewModel;

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToUserProfileCommand = ReactiveCommand.Create<Guid>(OnNavigateToUserProfile);
            AddParticipantCommand = ReactiveCommand.Create(OnAddParticipant);
            RemoveParticipantCommand = ReactiveCommand.Create<Guid>(OnRemoveParticipant);
            DeleteChatCommand = ReactiveCommand.Create(OnDeleteChat);
            LeaveChatCommand = ReactiveCommand.Create(OnLeaveChat);

            // Подписываемся на события
            _chatService.OnParticipantRemoved += OnParticipantRemovedEvent;
            _chatService.OnChatDeleted += OnChatDeleted;
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
        /// Является ли текущий пользователь администратором
        /// </summary>
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                this.RaiseAndSetIfChanged(ref _isAdmin, value);
                this.RaisePropertyChanged(nameof(IsNotAdmin));
            }
        }

        /// <summary>
        /// НЕ является ли текущий пользователь администратором (для UI binding)
        /// </summary>
        public bool IsNotAdmin => !IsAdmin;

        /// <summary>
        /// Коллекция участников чата
        /// </summary>
        public ObservableCollection<ParticipantViewModel> Participants { get; } = new();

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда перехода к профилю пользователя
        /// </summary>
        public ReactiveCommand<Guid, Unit> NavigateToUserProfileCommand { get; }

        /// <summary>
        /// Команда добавления участника
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddParticipantCommand { get; }

        /// <summary>
        /// Команда удаления участника
        /// </summary>
        public ReactiveCommand<Guid, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Команда удаления чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteChatCommand { get; }

        /// <summary>
        /// Команда выхода из чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> LeaveChatCommand { get; }

        /// <summary>
        /// Инициализация ViewModel - загрузка участников чата
        /// </summary>
        public async Task InitializeAsync()
        {
            if (CurrentChat == null)
            {
                System.Diagnostics.Debug.WriteLine("ParticipantsViewModel.InitializeAsync: CurrentChat is null");
                return;
            }

            var currentUserId = _authorizationService.UserId;
            if (currentUserId == null)
            {
                System.Diagnostics.Debug.WriteLine("ParticipantsViewModel.InitializeAsync: currentUserId is null");
                return;
            }

            // Проверяем, является ли текущий пользователь администратором
            IsAdmin = CurrentChat.AdminId == currentUserId.Value;
            System.Diagnostics.Debug.WriteLine($"ParticipantsViewModel.InitializeAsync: ChatId={CurrentChat.Id}, AdminId={CurrentChat.AdminId}, CurrentUserId={currentUserId}, IsAdmin={IsAdmin}");

            // Загружаем участников чата
            await LoadParticipantsAsync();
        }

        /// <summary>
        /// Загружает список участников чата
        /// </summary>
        private async Task LoadParticipantsAsync()
        {
            if (CurrentChat == null)
                return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"ParticipantsViewModel.LoadParticipantsAsync: Loading participants for chat {CurrentChat.Id}");
                var participants = await _chatService.GetChatParticipantsAsync(CurrentChat.Id);
                var currentUserId = _authorizationService.UserId;

                System.Diagnostics.Debug.WriteLine($"ParticipantsViewModel.LoadParticipantsAsync: Received {participants.Count()} participants");

                Participants.Clear();

                foreach (var participant in participants)
                {
                    var user = await _userService.GetUserByIdAsync(participant.UserId);
                    var isCurrentUser = participant.UserId == currentUserId;

                    var participantViewModel = new ParticipantViewModel
                    {
                        UserId = participant.UserId,
                        UserName = user?.UserName ?? user?.Login ?? "Неизвестный пользователь",
                        IsCurrentUser = isCurrentUser,
                        CanBeRemoved = IsAdmin && !isCurrentUser
                    };

                    System.Diagnostics.Debug.WriteLine($"ParticipantsViewModel.LoadParticipantsAsync: Added participant {participantViewModel.UserName} (IsCurrentUser={isCurrentUser}, CanBeRemoved={participantViewModel.CanBeRemoved})");

                    Participants.Add(participantViewModel);
                }

                System.Diagnostics.Debug.WriteLine($"ParticipantsViewModel.LoadParticipantsAsync: Total participants in collection: {Participants.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки участников: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
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
        private async void OnNavigateToUserProfile(Guid userId)
        {
            _userProfileViewModel.UserId = userId;
            await _userProfileViewModel.InitializeAsync();
            NavigationService.NavigateToWithHistory<UserProfileViewModel>();
        }

        /// <summary>
        /// Обработчик добавления участника
        /// </summary>
        private void OnAddParticipant()
        {
            NavigationService.NavigateToWithHistory<AddParticipantsViewModel>();
        }

        /// <summary>
        /// Обработчик удаления участника
        /// </summary>
        private async void OnRemoveParticipant(Guid userId)
        {
            if (CurrentChat == null || !IsAdmin)
                return;

            try
            {
                await _chatService.RemoveParticipantAsync(CurrentChat.Id, userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления участника: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик удаления чата
        /// </summary>
        private async void OnDeleteChat()
        {
            if (CurrentChat == null || !IsAdmin)
                return;

            try
            {
                await _chatService.DeleteChatAsync(CurrentChat);
                NavigationService.ClearNavigationStack();
                NavigationService.NavigateTo<ChatsViewModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления чата: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик выхода из чата
        /// </summary>
        private async void OnLeaveChat()
        {
            if (CurrentChat == null)
                return;

            var currentUserId = _authorizationService.UserId;
            if (currentUserId == null)
                return;

            try
            {
                await _chatService.RemoveParticipantAsync(CurrentChat.Id, currentUserId.Value);
                NavigationService.ClearNavigationStack();
                NavigationService.NavigateTo<ChatsViewModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка выхода из чата: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик события удаления участника
        /// </summary>
        private async void OnParticipantRemovedEvent(Guid chatId, Guid userId)
        {
            if (CurrentChat == null || CurrentChat.Id != chatId)
                return;

            // Если удалили текущего пользователя, возвращаемся в список чатов
            if (userId == _authorizationService.UserId)
            {
                NavigationService.ClearNavigationStack();
                NavigationService.NavigateTo<ChatsViewModel>();
                return;
            }

            // Иначе просто обновляем список участников
            await LoadParticipantsAsync();
        }

        private async void OnChatDeleted(Guid chatId)
        {
            if (chatId == CurrentChat?.Id)
            {
                NavigationService?.ClearNavigationStack();
                NavigationService?.NavigateTo<ChatsViewModel>();
                return;
            }
        }
    }
}
