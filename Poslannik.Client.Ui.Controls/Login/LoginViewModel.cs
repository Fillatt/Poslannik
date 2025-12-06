using DynamicData.Binding;
using Poslannik.Client.Services;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Client.Ui.Controls.ViewModels;
using ReactiveUI;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Threading.Tasks;

namespace Poslannik.Client.Ui.Controls;

/// <summary>ViewModel для экрана входа.</summary>
public class LoginViewModel : ViewModelBase
{
    private IAutorizationService _autorizationService;

    private string? _login;
    private string? _password;

    private bool _canLogin;

    /// <summary>Логин.</summary>
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string? Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    /// <summary>Пароль.</summary>
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string? Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    /// <summary>Флаг возможности входа.</summary>
    public bool CanLogin
    {
        get => _canLogin;
        set => this.RaiseAndSetIfChanged(ref _canLogin, value);
    }

    /// <summary>Команда входа в систему.</summary>
    public ReactiveCommand<Unit, Task> LoginCommand { get; }

    public LoginViewModel(IAutorizationService autorizationService)
    {
        _autorizationService = autorizationService;

        PropertyChanged += (o, e) => CanLogin = CheckCanLogin();

        LoginCommand = ReactiveCommand.Create(OnLoginAsync);
    }

    /// <summary>Обработчик входа в систему.</summary>
    private async Task OnLoginAsync()
    {
        var result = await _autorizationService.AuthorizeAsync(Login!, Password!, CancellationToken.None);
        if(result.IsSuccess) NavigationService?.NavigateTo<ChatsViewModel>();
    }

    private bool CheckCanLogin() => _login != null && _password != null;
}
