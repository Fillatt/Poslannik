using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Services;

public class UserService : IUserService
{
    private readonly string _url;
    private HubConnection? _hubConnection;
    private bool _isConnected;

    private IAutorizationService _autorizationService;

    public UserService(IConfiguration configuration, IAutorizationService autorizationService)
    {
        var apiUrl = configuration.GetRequiredSection("apiUrl").Value!;
        _url = apiUrl + HubConstants.UserHubPath;
        _autorizationService = autorizationService;
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string searchQuery, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            var users = await _hubConnection.InvokeAsync<IEnumerable<User>>(nameof(IUserHubRepository.SearchUsersAsync), searchQuery, cancellationToken);
            return users ?? Enumerable.Empty<User>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка поиска пользователей: {ex.Message}");
            return Enumerable.Empty<User>();
        }
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            var user = await _hubConnection.InvokeAsync<User?>(nameof(IUserHubRepository.GetUserByIdAsync), userId, cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка получения пользователя: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> ConnectAsync(string jwtToken, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                return true;
            }

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(jwtToken);
                    options.HttpMessageHandlerFactory = handler =>
                    {
                        if (handler is HttpClientHandler httpClientHandler)
                        {
                            httpClientHandler.ServerCertificateCustomValidationCallback =
                                (message, cert, chain, errors) => true;
                        }
                        return handler;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            await _hubConnection.StartAsync(cancellationToken);
            _isConnected = true;
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка подключения к UserHub: {ex.Message}");
            _isConnected = false;
            return false;
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync(cancellationToken);
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
            _isConnected = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка отключения от UserHub: {ex.Message}");
        }
    }
}
