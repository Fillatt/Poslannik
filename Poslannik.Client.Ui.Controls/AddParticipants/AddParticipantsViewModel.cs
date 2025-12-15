using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;
using System.Collections.ObjectModel;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для добавления участников в чат
    /// </summary>
    public class AddParticipantsViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly IAutorizationService _authorizationService;
        private readonly IUserService _userService;

        private Guid? _chatId;
        private string? _participantSearchQuery;
        private bool _isLoading;
        private string? _errorMessage;

        private ObservableCollection<User> _foundParticipants;
        private ObservableCollection<User> _participants;

        public AddParticipantsViewModel(IChatService chatService, IAutorizationService authorizationService, IUserService userService)
        {
            _chatService = chatService;
            _authorizationService = authorizationService;
            _userService = userService;

            _foundParticipants = new ObservableCollection<User>();
            _participants = new ObservableCollection<User>();

            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            RemoveParticipantCommand = ReactiveCommand.Create<User>(OnRemoveParticipant);
            AddCommand = ReactiveCommand.Create(OnAddAsync);
            AddParticipantCommand = ReactiveCommand.Create<User>(OnAddParticipant);
            SearchParticipantsCommand = ReactiveCommand.Create<string?, Task>(SearchParticipantsAsync);
        }

        /// <summary>
        /// ID текущего чата
        /// </summary>
        public Guid? ChatId
        {
            get => _chatId;
            set => this.RaiseAndSetIfChanged(ref _chatId, value);
        }

        /// <summary>
        /// Поисковый запрос для участников
        /// </summary>
        public string? ParticipantSearchQuery
        {
            get => _participantSearchQuery;
            set => this.RaiseAndSetIfChanged(ref _participantSearchQuery, value);
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
        /// Найденные участники для добавления
        /// </summary>
        public ObservableCollection<User> FoundParticipants
        {
            get => _foundParticipants;
            set => this.RaiseAndSetIfChanged(ref _foundParticipants, value);
        }

        /// <summary>
        /// Список выбранных участников для добавления
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
        /// Команда удаления участника из списка
        /// </summary>
        public ReactiveCommand<User, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Команда добавления участников в чат
        /// </summary>
        public ReactiveCommand<Unit, Task> AddCommand { get; }

        /// <summary>
        /// Команда добавления участника в список
        /// </summary>
        public ReactiveCommand<User, Unit> AddParticipantCommand { get; }

        /// <summary>
        /// Команда поиска участников
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
            ParticipantSearchQuery = null;
            ErrorMessage = null;
            FoundParticipants.Clear();
            Participants.Clear();
            this.RaisePropertyChanged(nameof(HasParticipants));
        }

        /// <summary>
        /// Поиск участников для добавления
        /// </summary>
        private async Task SearchParticipantsAsync(string? query)
        {
            if (string.IsNullOrWhiteSpace(query) || !ChatId.HasValue)
            {
                FoundParticipants.Clear();
                return;
            }

            try
            {
                // Получаем текущих участников чата
                var currentParticipants = await _chatService.GetChatParticipantsAsync(ChatId.Value);
                var currentUserIds = currentParticipants.Select(p => p.UserId).ToHashSet();

                // Ищем пользователей
                var users = await _userService.SearchUsersAsync(query);
                FoundParticipants.Clear();

                // Фильтруем: исключаем текущего пользователя, уже добавленных в чат и выбранных для добавления
                foreach (var user in users.Where(u =>
                    u.Id != _authorizationService.UserId &&
                    !currentUserIds.Contains(u.Id) &&
                    !_participants.Any(p => p.Id == u.Id)))
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
        /// Добавление участника в список для добавления
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
        /// Обработчик удаления участника из списка
        /// </summary>
        private void OnRemoveParticipant(User user)
        {
            Participants.Remove(user);
            this.RaisePropertyChanged(nameof(HasParticipants));
        }

        /// <summary>
        /// Обработчик добавления участников в чат
        /// </summary>
        private async Task OnAddAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            ErrorMessage = null;

            try
            {
                if (!ChatId.HasValue)
                {
                    ErrorMessage = "Чат не выбран";
                    return;
                }

                if (Participants.Count == 0)
                {
                    ErrorMessage = "Добавьте хотя бы одного участника";
                    return;
                }

                var participantIds = Participants.Select(p => p.Id).ToList();
                await _chatService.AddParticipantsAsync(ChatId.Value, participantIds);

                // Сбрасываем состояние и возвращаемся назад
                ResetState();
                NavigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при добавлении участников";
                System.Diagnostics.Debug.WriteLine($"Ошибка добавления участников: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
