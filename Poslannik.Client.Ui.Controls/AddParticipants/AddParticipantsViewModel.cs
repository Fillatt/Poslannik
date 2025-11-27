using System.Reactive;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// ViewModel для добавления участников в чат
    /// </summary>
    public class AddParticipantsViewModel : ViewModelBase
    {
        public AddParticipantsViewModel()
        {
            NavigateBackCommand = ReactiveCommand.Create(OnNavigateBack);
            RemoveParticipantCommand = ReactiveCommand.Create(OnRemoveParticipant);
            AddCommand = ReactiveCommand.Create(OnAdd);
        }

        /// <summary>
        /// Команда возврата назад
        /// </summary>
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }

        /// <summary>
        /// Команда удаления участника
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveParticipantCommand { get; }

        /// <summary>
        /// Команда добавления участников
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        /// <summary>
        /// Обработчик возврата назад
        /// </summary>
        private void OnNavigateBack()
        {
            NavigationService.NavigateBack();
        }

        /// <summary>
        /// Обработчик удаления участника
        /// </summary>
        private void OnRemoveParticipant()
        {
        }

        /// <summary>
        /// Обработчик добавления участников
        /// </summary>
        private void OnAdd()
        {
            NavigationService.NavigateBack();
        }
    }
}
