using ReactiveUI;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для отдельного участника группового чата
    /// </summary>
    public class ParticipantViewModel : ViewModelBase
    {
        private Guid _userId;
        private string _userName = string.Empty;
        private bool _isCurrentUser;
        private bool _canBeRemoved;

        /// <summary>
        /// ID пользователя
        /// </summary>
        public Guid UserId
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
        /// Флаг, является ли это текущий пользователь
        /// </summary>
        public bool IsCurrentUser
        {
            get => _isCurrentUser;
            set => this.RaiseAndSetIfChanged(ref _isCurrentUser, value);
        }

        /// <summary>
        /// Флаг, можно ли удалить этого участника (не текущий пользователь и текущий пользователь - админ)
        /// </summary>
        public bool CanBeRemoved
        {
            get => _canBeRemoved;
            set => this.RaiseAndSetIfChanged(ref _canBeRemoved, value);
        }
    }
}
