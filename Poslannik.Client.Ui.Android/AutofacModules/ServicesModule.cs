using Autofac;
using Poslannik.Client.Services;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android.AutofacModules;

public sealed class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<NavigationService>()
            .As<INavigationService>()
            .AsSelf()
            .SingleInstance();

        builder
            .RegisterType<AuthorizationService>()
            .As<IAutorizationService>()
            .AsSelf();

        builder
            .RegisterType<ChatService>()
            .As<IChatService>()
            .AsSelf()
            .SingleInstance();
    }
}
