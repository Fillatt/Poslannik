using Poslannik.Framework.Models;

namespace Poslannik.Client.Services.Interfaces;

public interface IChatService
{
    bool IsConnected { get; }

    event Action<Chat>? OnChatCreated;
    event Action<Chat>? OnChatUpdated;
    event Action<Guid>? OnChatDeleted;

    Task<bool> ConnectAsync(string jwtToken, CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Chat?> CreateChatAsync(Chat chat, CancellationToken cancellationToken = default);
    Task UpdateChatAsync(Chat chat, CancellationToken cancellationToken = default);
    Task DeleteChatAsync(Guid chatId, CancellationToken cancellationToken = default);
}
