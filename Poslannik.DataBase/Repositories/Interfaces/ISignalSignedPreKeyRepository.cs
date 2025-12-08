using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface ISignalSignedPreKeyRepository
{
    Task<SignalSignedPreKey?> GetBySignedPreKeyIdAsync(Guid userId, int signedPreKeyId);
    Task<SignalSignedPreKey?> GetLatestAsync(Guid userId);
    Task AddAsync(SignalSignedPreKey signedPreKey);
}
