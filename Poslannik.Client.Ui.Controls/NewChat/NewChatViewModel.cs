using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для создания нового чата
    /// </summary>
    public class NewChatViewModel : ViewModelBase
    {
        public NewChatViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            CreateChatCommand = ReactiveCommand.Create(OnCreateChat);
            RemoveParticipantCommand = ReactiveCommand.Create(OnRemoveParticipant);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда создания чата
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateChatCommand { get; }

        /// <summary>
        /// Команда удаления участника
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }

        /// <summary>
        /// Обработчик создания чата
        /// </summary>
        private void OnCreateChat()
        {
            NavigationService.ClearNavigationStack();
            NavigationService.NavigateToWithHistory<ChatViewModel>();
        }

        /// <summary>
        /// Обработчик удаления участника
        /// </summary>
        private void OnRemoveParticipant()
        {
        }
    }
}
