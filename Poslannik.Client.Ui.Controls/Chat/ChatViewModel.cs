using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для личного чата
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        private Chat? _currentChat;

        public ChatViewModel()
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToUserProfileCommand = ReactiveCommand.Create(OnNavigateToUserProfile);
            SendMessageCommand = ReactiveCommand.Create(OnSendMessage);
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
            NavigationService.NavigateToWithHistory<UserProfileViewModel>();
        }

        /// <summary>
        /// Обработчик отправки сообщения
        /// </summary>
        private void OnSendMessage()
        {
        }
    }
}
