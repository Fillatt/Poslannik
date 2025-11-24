using Microsoft.AspNetCore.SignalR.Client;
using Poslannik.Framework.Requests;
using Poslannik.Framework.Responses;
namespace Poslannik.Client.Services
{
    public class AuthorizationService
    {
        string? JwtToken { get; set; }
        bool IsAuthorizated { get; set; }
        
        string HubUrl { get; set; }

        HubConnection HubConnection { get; set; }

        public AuthorizationService(string hubUrl) {
            HubUrl = hubUrl;
            InitializeConnection();
        }

        private void InitializeConnection() {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(HubUrl)
                .WithAutomaticReconnect() // Автоматическое переподключение
                .Build();
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (HubConnection.State == HubConnectionState.Disconnected)
                {
                    await HubConnection.StartAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }

        public async Task<AuthorizationResponse> AuthorizeAsync(string login, string password)
        {
            try
            {
                if (HubConnection.State != HubConnectionState.Connected)
                {
                    var connected = await ConnectAsync();
                    if (!connected) return new AuthorizationResponse { IsSuccess = false};
                }

                var request = new AuthorizationRequest { Login = login, Password = password };
                var response = await HubConnection.InvokeAsync<AuthorizationResponse>("AuthorizeAsync", request);

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
            catch (Exception)
            {
                JwtToken = null;
                IsAuthorizated = false;
                return new AuthorizationResponse { IsSuccess = false};
            }
        }

        public void Logout()
        {
            JwtToken = null;
            IsAuthorizated = false;
        }
    }
}

