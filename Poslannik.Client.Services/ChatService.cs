using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Client.Services;

public class ChatService : IChatService
{
    private HubConnection? _hubConnection;
    private readonly string _url;
    private bool _isConnected;
    private IAutorizationService _autorizationService;

    public bool IsConnected => _isConnected && _hubConnection?.State == HubConnectionState.Connected;

    public event Action<Chat>? OnChatCreated;
    public event Action<Chat>? OnChatUpdated;
    public event Action<Guid>? OnChatDeleted;

    public ChatService(IConfiguration configuration, IAutorizationService authorizationService)
    {
        var apiUrl = configuration.GetRequiredSection("apiUrl").Value!;
        _url = apiUrl + HubConstants.ChatHubPath;
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
            System.Diagnostics.Debug.WriteLine($"Ошибка подключения к ChatHub: {ex.Message}");
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
            System.Diagnostics.Debug.WriteLine($"Ошибка отключения от ChatHub: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Chat>> GetUserChatsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            var chats = await _hubConnection.InvokeAsync<IEnumerable<Chat>>(
                nameof(IChatHub.GetUserChatsAsync),
                _autorizationService.UserId,
                cancellationToken);

            return chats ?? Enumerable.Empty<Chat>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка получения чатов: {ex.Message}");
            return Enumerable.Empty<Chat>();
        }
    }

    public async Task<Chat?> CreateChatAsync(Chat chat, IEnumerable<Guid>? participantUserIds = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            var createdChat = await _hubConnection.InvokeAsync<Chat>(
                nameof(IChatHub.CreateChatAsync),
                chat,
                participantUserIds,
                cancellationToken);

            return createdChat;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка создания чата: {ex.Message}");
            return null;
        }
    }

    public async Task UpdateChatAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                await ConnectAsync(_autorizationService.JwtToken, cancellationToken);
            }

            await _hubConnection.InvokeAsync(
                nameof(IChatHub.UpdateChatAsync),
                chat,
                cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка обновления чата: {ex.Message}");
        }
    }

    public async Task DeleteChatAsync(Guid chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                throw new InvalidOperationException("Не подключено к ChatHub");
            }

            await _hubConnection.InvokeAsync(
                nameof(IChatHub.DeleteChatAsync),
                chatId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка удаления чата: {ex.Message}");
        }
    }

    private void RegisterEventHandlers()
    {
        if (_hubConnection == null) return;

        _hubConnection.On<Chat>(HubConstants.ChatEvents.ChatCreated, chat =>
        {
            OnChatCreated?.Invoke(chat);
        });

        _hubConnection.On<Chat>(HubConstants.ChatEvents.ChatUpdated, chat =>
        {
            OnChatUpdated?.Invoke(chat);
        });

        _hubConnection.On<Guid>(HubConstants.ChatEvents.ChatDeleted, chatId =>
        {
            OnChatDeleted?.Invoke(chatId);
        });

        _hubConnection.Reconnecting += error =>
        {
            _isConnected = false;
            System.Diagnostics.Debug.WriteLine($"Переподключение к ChatHub: {error?.Message}");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            _isConnected = true;
            System.Diagnostics.Debug.WriteLine($"Переподключено к ChatHub: {connectionId}");
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            _isConnected = false;
            System.Diagnostics.Debug.WriteLine($"Соединение с ChatHub закрыто: {error?.Message}");
            return Task.CompletedTask;
        };
    }
}
