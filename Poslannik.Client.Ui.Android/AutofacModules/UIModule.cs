using Autofac;
using Poslannik.Client.Ui.Controls;
using Poslannik.Client.Ui.Controls.Services;

namespace Poslannik.Client.Ui.Android.AutofacModules;

/// <summary>Модуль регистрации компонентов UI.</summary>
public sealed class UIModule : Module
{
    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LoginViewModel>().SingleInstance();
        builder.RegisterType<ChatsViewModel>().SingleInstance();
        builder.RegisterType<ProfileViewModel>().SingleInstance();
        builder.RegisterType<ChatViewModel>().SingleInstance();
        builder.RegisterType<GroupChatViewModel>();
        builder.RegisterType<NewChatViewModel>();
        builder.RegisterType<ParticipantsViewModel>();
        builder.RegisterType<UserProfileViewModel>();
        builder.RegisterType<AddParticipantsViewModel>();

        builder.RegisterType<MainViewModel>().SingleInstance();
    }
}
