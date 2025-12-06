using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для списка чатов
    /// </summary>
    public class ChatsViewModel : ViewModelBase
    {
        public ChatsViewModel()
        {
            NavigateToProfileCommand = ReactiveCommand.Create(OnNavigateToProfile);
            NavigateToChatCommand = ReactiveCommand.Create(OnNavigateToChat);
            NavigateToGroupChatCommand = ReactiveCommand.Create(OnNavigateToGroupChat);
            NavigateToNewChatCommand = ReactiveCommand.Create(OnNavigateToNewChat);
        }

        /// <summary>
        /// Команда перехода к профилю
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToProfileCommand { get; }

        /// <summary>
        /// Команда перехода к чату
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToChatCommand { get; }

        /// <summary>
        /// Команда перехода к групповому чату
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateToGroupChatCommand { get; }

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
        private void OnNavigateToChat()
        {
            NavigationService.NavigateToWithHistory<ChatViewModel>();
        }

        /// <summary>
        /// Обработчик перехода к групповому чату
        /// </summary>
        private void OnNavigateToGroupChat()
        {
            NavigationService.NavigateToWithHistory<GroupChatViewModel>();
        }

        /// <summary>
        /// Обработчик перехода к созданию нового чата
        /// </summary>
        private void OnNavigateToNewChat()
        {
            NavigationService.NavigateToWithHistory<NewChatViewModel>();
        }
    }
}
