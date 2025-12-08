using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface ISignalPreKeyRepository
{
    Task<SignalPreKey?> GetUnusedPreKeyAsync(Guid userId);
    Task<SignalPreKey?> GetByPreKeyIdAsync(Guid userId, int preKeyId);
    Task AddRangeAsync(IEnumerable<SignalPreKey> preKeys);
    Task MarkAsUsedAsync(Guid userId, int preKeyId);
    Task<int> GetUnusedCountAsync(Guid userId);
}
