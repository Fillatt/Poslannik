using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Poslannik.Client.Services.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Requests;
using Poslannik.Framework.Responses;

namespace Poslannik.Client.Services;

public class AuthorizationService : IAutorizationService
{
    private HubConnection _hubConnection;
    private string _url;

    public string? JwtToken { get; set; }

    public bool IsAuthorizated { get; set; }

    public AuthorizationService(IConfiguration configuration)
    {
        var apiUrl = configuration.GetRequiredSection("apiUrl").Value!;
        _url = apiUrl + HubConstants.AuthorizationHubPath;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_url, options =>
            {
                options.HttpMessageHandlerFactory = handler =>
                {
                    if (handler is HttpClientHandler httpClientHandler)
                    {
                        // Отключаем проверку SSL
                        httpClientHandler.ServerCertificateCustomValidationCallback =
                            (message, cert, chain, errors) => true;
                    }
                    return handler;
                };
            })
            .WithAutomaticReconnect() // Автоматическое переподключение
            .Build();
    }

    public async Task<AuthorizationResponse> AuthorizeAsync(string login, string password, CancellationToken cancellationToken)
    {
        try
        {
            if (_hubConnection.State != HubConnectionState.Connected)
            {
                var connected = await ConnectAsync(cancellationToken);
                if (!connected) return new AuthorizationResponse { IsSuccess = false};
            }

            var request = new AuthorizationRequest { Login = login, Password = password };
            var response = await _hubConnection.InvokeAsync<AuthorizationResponse>("AuthorizeAsync", request);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.Token))
            {
                JwtToken = response.Token;
                IsAuthorizated = true;
            }
            else
            {
                JwtToken = null;
                IsAuthorizated = false;
            }

            return response;
        }
        catch (Exception ex)
        {
            JwtToken = null;
            IsAuthorizated = false;
            return new AuthorizationResponse { IsSuccess = false};
        }
    }

    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        JwtToken = null;
        IsAuthorizated = false;
    }

    private async Task<bool> ConnectAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync(cancellationToken);
            }
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка подключения: {ex.Message}");
            return false;
        }
    }
}

