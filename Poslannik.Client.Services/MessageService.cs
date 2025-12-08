using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Services;

public class MessageService : IMessageService
{
    private HubConnection? _hubConnection;
    private readonly string _url;
    private bool _isConnected;
    private IAutorizationService _autorizationService;

    public event Action<Message>? OnMessageSended;

    public MessageService(IConfiguration configuration, IAutorizationService authorizationService)
    {
        var apiUrl = configuration.GetRequiredSection("apiUrl").Value!;
        _url = apiUrl + HubConstants.MessageHubPath;
        _autorizationService = authorizationService;
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

            RegisterEventHandlers();

            await _hubConnection.StartAsync(cancellationToken);
            _isConnected = true;
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка подключения к MessageHub: {ex.Message}");
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
            System.Diagnostics.Debug.WriteLine($"Ошибка отключения от MessageHub: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            await _hubConnection.SendAsync(nameof(IMessageHub.SendMessageAsync), message, cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка получения чатов: {ex.Message}");
        }
    }

    private void RegisterEventHandlers()
    {
        if (_hubConnection == null) return;

        _hubConnection.On<Message>(HubConstants.MessageEvents.MessageSended, message =>
        {
            OnMessageSended?.Invoke(message);
        });

        _hubConnection.Reconnecting += error =>
        {
            _isConnected = false;
            System.Diagnostics.Debug.WriteLine($"Переподключение к MessageHub: {error?.Message}");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            _isConnected = true;
            System.Diagnostics.Debug.WriteLine($"Переподключено к MessageHub: {connectionId}");
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            _isConnected = false;
            System.Diagnostics.Debug.WriteLine($"Соединение с MessageHub закрыто: {error?.Message}");
            return Task.CompletedTask;
        };
    }
}
