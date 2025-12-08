using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface ISignalSessionRepository
{
    Task<SignalSession?> GetSessionAsync(Guid userId, Guid recipientId, int deviceId);
    Task SaveSessionAsync(SignalSession session);
    Task DeleteSessionAsync(Guid userId, Guid recipientId, int deviceId);
    Task<bool> ExistsAsync(Guid userId, Guid recipientId, int deviceId);
}
