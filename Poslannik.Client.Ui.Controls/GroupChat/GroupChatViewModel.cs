using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для группового чата
    /// </summary>
    public class GroupChatViewModel : ViewModelBase
    {
        public GroupChatViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToParticipantsCommand = ReactiveCommand.Create(OnNavigateToParticipants);
            DeleteChatCommand = ReactiveCommand.Create(OnDeleteChat);
            LeaveChatCommand = ReactiveCommand.Create(OnLeaveChat);
            SendMessageCommand = ReactiveCommand.Create(OnSendMessage);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда перехода к списку участников
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToParticipantsCommand { get; }

        /// <summary>
        /// Команда удаления чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteChatCommand { get; }

        /// <summary>
        /// Команда выхода из чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> LeaveChatCommand { get; }

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
        /// Обработчик перехода к списку участников
        /// </summary>
        private void OnNavigateToParticipants()
        {
            NavigationService.NavigateToWithHistory<ParticipantsViewModel>();
        }

        /// <summary>
        /// Обработчик удаления чата
        /// </summary>
        private void OnDeleteChat()
        {
            NavigationService.ClearNavigationStack();
            NavigationService.NavigateTo<ChatsViewModel>();
        }

        /// <summary>
        /// Обработчик выхода из чата
        /// </summary>
        private void OnLeaveChat()
        {
            NavigationService.ClearNavigationStack();
            NavigationService.NavigateTo<ChatsViewModel>();
        }

        /// <summary>
        /// Обработчик отправки сообщения
        /// </summary>
        private void OnSendMessage()
        {
        }
    }
}
