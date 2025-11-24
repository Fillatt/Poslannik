using System;

namespace Poslannik.Client.Ui.Controls.Services
{
    /// <summary>
    /// Сервис навигации между представлениями
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Текущая ViewModel
        /// </summary>
        IObservable<object?> CurrentViewModel { get; }

        /// <summary>
        /// Навигация к указанной ViewModel
        /// </summary>
        void NavigateTo<TViewModel>() where TViewModel : class;

        /// <summary>
        /// Навигация к указанной ViewModel с сохранением истории
        /// </summary>
        void NavigateToWithHistory<TViewModel>() where TViewModel : class;

        /// <summary>
        /// Возврат к предыдущему представлению
        /// </summary>
        void NavigateBack();

        /// <summary>
        /// Проверка возможности возврата назад
        /// </summary>
        bool CanNavigateBack { get; }

        /// <summary>
        /// Очистка стека навигации
        /// </summary>
        void ClearNavigationStack();
    }
}
