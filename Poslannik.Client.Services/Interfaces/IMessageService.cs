using Poslannik.Framework.Models;

namespace Poslannik.Client.Services.Interfaces;

public interface IMessageService
{
    bool IsConnected { get; }

    event Action<Message>? OnMessageReceived;
    event Action<Message>? OnMessageSent;
    event Action<Guid>? OnMessageDeleted;
    event Action<Message>? OnMessageUpdated;

    Task<bool> ConnectAsync(string jwtToken, CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Message>> GetChatMessagesAsync(Guid chatId, CancellationToken cancellationToken = default);
    Task<Message?> SendMessageAsync(Guid chatId, string messageText, CancellationToken cancellationToken = default);
}
