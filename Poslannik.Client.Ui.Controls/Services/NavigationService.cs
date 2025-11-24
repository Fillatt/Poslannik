using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Poslannik.Client.Ui.Controls.Services
{
    /// <summary>
    /// Реализация сервиса навигации
    /// </summary>
    public class NavigationService : ReactiveObject, INavigationService
    {
        private readonly Dictionary<Type, Func<object>> _viewModelFactory;
        private readonly Stack<object> _navigationStack;
        private object? _currentViewModel;

        public NavigationService()
        {
            _viewModelFactory = new Dictionary<Type, Func<object>>();
            _navigationStack = new Stack<object>();
        }

        /// <summary>
        /// Текущая ViewModel
        /// </summary>
        public IObservable<object?> CurrentViewModel => this.WhenAnyValue(x => x.CurrentViewModelValue);

        private object? CurrentViewModelValue
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        /// <summary>
        /// Регистрация фабрики для создания ViewModel
        /// </summary>
        public void RegisterViewModel<TViewModel>(Func<TViewModel> factory) where TViewModel : class
        {
            _viewModelFactory[typeof(TViewModel)] = () => factory();
        }

        /// <summary>
        /// Навигация к указанной ViewModel без сохранения истории
        /// </summary>
        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            if (_viewModelFactory.TryGetValue(typeof(TViewModel), out var factory))
            {
                CurrentViewModelValue = factory();
            }
            else
            {
                throw new InvalidOperationException($"ViewModel {typeof(TViewModel).Name} не зарегистрирована в NavigationService");
            }
        }

        /// <summary>
        /// Навигация к указанной ViewModel с сохранением текущей в истории
        /// </summary>
        public void NavigateToWithHistory<TViewModel>() where TViewModel : class
        {
            if (_currentViewModel != null)
            {
                _navigationStack.Push(_currentViewModel);
            }

            NavigateTo<TViewModel>();
        }

        /// <summary>
        /// Возврат к предыдущему представлению
        /// </summary>
        public void NavigateBack()
        {
            if (_navigationStack.Count > 0)
            {
                CurrentViewModelValue = _navigationStack.Pop();
            }
        }

        /// <summary>
        /// Проверка возможности возврата назад
        /// </summary>
        public bool CanNavigateBack => _navigationStack.Count > 0;

        /// <summary>
        /// Очистка стека навигации
        /// </summary>
        public void ClearNavigationStack()
        {
            _navigationStack.Clear();
        }
    }
}
