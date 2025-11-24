using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Poslannik.Client.Ui.Controls;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android
{
    public partial class App : Application
    {
        private NavigationService? _navigationService;
        private MainViewModel? _mainViewModel;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                // Создание и настройка NavigationService
                _navigationService = new NavigationService();

                // Регистрация всех ViewModels в NavigationService
                _navigationService.RegisterViewModel(() => new LoginViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new ChatsViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new ProfileViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new ChatViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new GroupChatViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new NewChatViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new ParticipantsViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new UserProfileViewModel(_navigationService));
                _navigationService.RegisterViewModel(() => new AddParticipantsViewModel(_navigationService));

                // Создание MainViewModel
                _mainViewModel = new MainViewModel(_navigationService);

                // Создание и настройка MainView
                var mainView = new MainView
                {
                    DataContext = _mainViewModel
                };

                // Инициализация приложения (переход к LoginView)
                _mainViewModel.Initialize();

                singleViewPlatform.MainView = mainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}