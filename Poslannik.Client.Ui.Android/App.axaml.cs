using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Microsoft.Extensions.Configuration;
using Poslannik.Client.Ui.Android.AutofacModules;
using Poslannik.Client.Ui.Controls;
using System.IO;
using System.Reflection;

namespace Poslannik.Client.Ui.Android;

public partial class App : Application
{
    public static IContainer? Container { get; private set; } 

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Poslannik.Client.Ui.Android.appsettings.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);

            var configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder
                .RegisterInstance(configuration)
                .As<IConfiguration>()
                .AsSelf();

            builder.RegisterModule<UIModule>();
            builder.RegisterModule<ServicesModule>();

            Container = builder.Build();

            // Создание и настройка MainView
            var mainViewModel = Container.Resolve<MainViewModel>();
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
