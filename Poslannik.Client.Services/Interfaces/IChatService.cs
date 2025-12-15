using Poslannik.Framework.Models;

namespace Poslannik.Client.Services.Interfaces;

public interface IChatService
{
    bool IsConnected { get; }

    event Action<Chat>? OnChatCreated;
    event Action<Chat>? OnChatUpdated;
    event Action<Guid>? OnChatDeleted;
    event Action<Guid, Guid>? OnParticipantRemoved;
    event Action<Guid, Guid>? OnAdminRightsTransferred;

    Task<bool> ConnectAsync(string jwtToken, CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Chat>> GetUserChatsAsync(CancellationToken cancellationToken = default);
    Task<Chat?> CreateChatAsync(Chat chat, IEnumerable<Guid>? participantUserIds = null, CancellationToken cancellationToken = default);
    Task UpdateChatAsync(Chat chat, CancellationToken cancellationToken = default);
    Task DeleteChatAsync(Guid chatId, CancellationToken cancellationToken = default);

    Task<IEnumerable<ChatParticipant>> GetChatParticipantsAsync(Guid chatId, CancellationToken cancellationToken = default);
    Task RemoveParticipantAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
    Task AddParticipantsAsync(Guid chatId, IEnumerable<Guid> participantUserIds, CancellationToken cancellationToken = default);
}
