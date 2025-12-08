using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface ISignalIdentityKeyRepository
{
    Task<SignalIdentityKey?> GetByUserIdAsync(Guid userId);
    Task AddAsync(SignalIdentityKey identityKey);
    Task<bool> ExistsAsync(Guid userId);
}
