using Poslannik.Framework.Models;

namespace Poslannik.Client.Services.Interfaces;

public interface IMessageService
{
    event Action<Message>? OnMessageSended;

    Task<bool> ConnectAsync(string jwtToken, CancellationToken cancellationToken = default);

    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task SendMessageAsync(Message message, CancellationToken cancellationToken = default);
}
