using System;
using ReactiveUI;
using Poslannik.Client.Ui.Controls.Services;
using Poslannik.Client.Ui.Controls.ViewModels;

namespace Poslannik.Client.Ui.Controls
{
    /// <summary>
    /// Главная ViewModel приложения, управляющая навигацией
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private object? _currentViewModel;

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            navigationService.CurrentViewModel.Subscribe(vm => CurrentViewModel = vm);
        }

        /// <summary>
        /// Текущая отображаемая ViewModel
        /// </summary>
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        /// <summary>
        /// Инициализация главной ViewModel
        /// </summary>
        public void Initialize()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }
    }
}
