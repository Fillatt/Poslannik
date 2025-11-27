using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для списка участников группового чата
    /// </summary>
    public class ParticipantsViewModel : ViewModelBase
    {
        public ParticipantsViewModel()
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            NavigateToUserProfileCommand = ReactiveCommand.Create(OnNavigateToUserProfile);
            AddParticipantCommand = ReactiveCommand.Create(OnAddParticipant);
            RemoveParticipantCommand = ReactiveCommand.Create(OnRemoveParticipant);
            DeleteChatCommand = ReactiveCommand.Create(OnDeleteChat);
            LeaveChatCommand = ReactiveCommand.Create(OnLeaveChat);
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
        /// Команда добавления участника
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddParticipantCommand { get; }

        /// <summary>
        /// Команда удаления участника
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Команда удаления чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteChatCommand { get; }

        /// <summary>
        /// Команда выхода из чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> LeaveChatCommand { get; }

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
        /// Обработчик добавления участника
        /// </summary>
        private void OnAddParticipant()
        {
            NavigationService.NavigateToWithHistory<AddParticipantsViewModel>();
        }

        /// <summary>
        /// Обработчик удаления участника
        /// </summary>
        private void OnRemoveParticipant()
        {
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
    }
}
