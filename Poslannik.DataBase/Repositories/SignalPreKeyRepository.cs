using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories;

public class SignalPreKeyRepository : ISignalPreKeyRepository
{
    private readonly ApplicationContext _context;

    public SignalPreKeyRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SignalPreKey?> GetUnusedPreKeyAsync(Guid userId)
    {
        var entity = await _context.SignalPreKeys
            .Where(x => x.UserId == userId && !x.IsUsed)
            .OrderBy(x => x.PreKeyId)
            .FirstOrDefaultAsync();

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task<SignalPreKey?> GetByPreKeyIdAsync(Guid userId, int preKeyId)
    {
        var entity = await _context.SignalPreKeys
            .FirstOrDefaultAsync(x => x.UserId == userId && x.PreKeyId == preKeyId);

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task AddRangeAsync(IEnumerable<SignalPreKey> preKeys)
    {
        var entities = preKeys.Select(MapToEntity);
        await _context.SignalPreKeys.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsUsedAsync(Guid userId, int preKeyId)
    {
        var entity = await _context.SignalPreKeys
            .FirstOrDefaultAsync(x => x.UserId == userId && x.PreKeyId == preKeyId);

        if (entity != null)
        {
            entity.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetUnusedCountAsync(Guid userId)
    {
        return await _context.SignalPreKeys
            .Where(x => x.UserId == userId && !x.IsUsed)
            .CountAsync();
    }

    private SignalPreKeyEntity MapToEntity(SignalPreKey model)
    {
        return new SignalPreKeyEntity
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            PreKeyId = model.PreKeyId,
            PublicKey = model.PublicKey,
            PrivateKey = model.PrivateKey,
            IsUsed = model.IsUsed
        };
    }

    private SignalPreKey MapToModel(SignalPreKeyEntity entity)
    {
        return new SignalPreKey
        {
            UserId = entity.UserId,
            PreKeyId = entity.PreKeyId,
            PublicKey = entity.PublicKey,
            PrivateKey = entity.PrivateKey,
            IsUsed = entity.IsUsed
        };
    }
}
