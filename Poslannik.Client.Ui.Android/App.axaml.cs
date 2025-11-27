using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Poslannik.Client.Ui.Android.AutofacModules;
using Poslannik.Client.Ui.Controls;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android;

public partial class App : Application
{
    private IContainer? _container;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<UIModule>();
            builder.RegisterModule<ServicesModule>();

            _container = builder.Build();

            // Создание и настройка MainView
            var mainViewModel = _container.Resolve<MainViewModel>();
            var mainView = new MainView
            {
                DataContext = mainViewModel
            };

            // Инициализация приложения (переход к LoginView)
            mainViewModel.Initialize();

            singleViewPlatform.MainView = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
