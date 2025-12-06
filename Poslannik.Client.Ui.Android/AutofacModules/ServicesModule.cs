using Autofac;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android.AutofacModules;

public sealed class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<NavigationService>()
            .As<INavigationService>()
            .AsSelf();
    }
}
