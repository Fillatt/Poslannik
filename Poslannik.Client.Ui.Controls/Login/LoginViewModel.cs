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

    private ChatsViewModel _chatsViewModel;
    private string? _login;
    private string? _password;

    private bool _canLogin;
    private bool _isLoading;
    private string? _errorMessage;

    /// <summary>Логин.</summary>
    public string? Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    /// <summary>Пароль.</summary>
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

    /// <summary>Сообщение об ошибке.</summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    /// <summary>Флаг процесса загрузки.</summary>
    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    /// <summary>Команда входа в систему.</summary>
    public ReactiveCommand<Unit, Task> LoginCommand { get; }

    public LoginViewModel(IAutorizationService autorizationService, ChatsViewModel chatsViewModel)
    {
        _chatsViewModel = chatsViewModel;

        _autorizationService = autorizationService;

        PropertyChanged += (o, e) => CanLogin = CheckCanLogin();

        LoginCommand = ReactiveCommand.Create(OnLoginAsync);
    }

    /// <summary>Обработчик входа в систему.</summary>
    private async Task OnLoginAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var result = await _autorizationService.AuthorizeAsync(Login!, Password!, CancellationToken.None);
            if (result.IsSuccess)
            {
                await _chatsViewModel.InitializeAsync();
                NavigationService?.NavigateTo<ChatsViewModel>();
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка подключения к серверу";
            System.Diagnostics.Debug.WriteLine($"Ошибка авторизации: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool CheckCanLogin() => _login != null && _password != null && !_isLoading;
}
