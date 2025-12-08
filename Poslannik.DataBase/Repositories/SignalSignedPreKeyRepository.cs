using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories;

public class SignalSignedPreKeyRepository : ISignalSignedPreKeyRepository
{
    private readonly ApplicationContext _context;

    public SignalSignedPreKeyRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SignalSignedPreKey?> GetBySignedPreKeyIdAsync(Guid userId, int signedPreKeyId)
    {
        var entity = await _context.SignalSignedPreKeys
            .FirstOrDefaultAsync(x => x.UserId == userId && x.SignedPreKeyId == signedPreKeyId);

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task<SignalSignedPreKey?> GetLatestAsync(Guid userId)
    {
        var entity = await _context.SignalSignedPreKeys
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync();

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task AddAsync(SignalSignedPreKey signedPreKey)
    {
        var entity = MapToEntity(signedPreKey);
        await _context.SignalSignedPreKeys.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    private SignalSignedPreKeyEntity MapToEntity(SignalSignedPreKey model)
    {
        return new SignalSignedPreKeyEntity
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            SignedPreKeyId = model.SignedPreKeyId,
            PublicKey = model.PublicKey,
            PrivateKey = model.PrivateKey,
            Signature = model.Signature,
            Timestamp = model.Timestamp
        };
    }

    private SignalSignedPreKey MapToModel(SignalSignedPreKeyEntity entity)
    {
        return new SignalSignedPreKey
        {
            UserId = entity.UserId,
            SignedPreKeyId = entity.SignedPreKeyId,
            PublicKey = entity.PublicKey,
            PrivateKey = entity.PrivateKey,
            Signature = entity.Signature,
            Timestamp = entity.Timestamp
        };
    }
}
