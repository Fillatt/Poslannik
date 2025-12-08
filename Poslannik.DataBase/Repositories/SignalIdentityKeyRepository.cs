using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories;

public class SignalIdentityKeyRepository : ISignalIdentityKeyRepository
{
    private readonly ApplicationContext _context;

    public SignalIdentityKeyRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SignalIdentityKey?> GetByUserIdAsync(Guid userId)
    {
        var entity = await _context.SignalIdentityKeys
            .FirstOrDefaultAsync(x => x.UserId == userId);

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task AddAsync(SignalIdentityKey identityKey)
    {
        var entity = MapToEntity(identityKey);
        await _context.SignalIdentityKeys.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId)
    {
        return await _context.SignalIdentityKeys.AnyAsync(x => x.UserId == userId);
    }

    private SignalIdentityKeyEntity MapToEntity(SignalIdentityKey model)
    {
        return new SignalIdentityKeyEntity
        {
            UserId = model.UserId,
            PublicKey = model.PublicKey,
            PrivateKey = model.PrivateKey,
            RegistrationId = model.RegistrationId
        };
    }

    private SignalIdentityKey MapToModel(SignalIdentityKeyEntity entity)
    {
        return new SignalIdentityKey
        {
            UserId = entity.UserId,
            PublicKey = entity.PublicKey,
            PrivateKey = entity.PrivateKey,
            RegistrationId = entity.RegistrationId
        };
    }
}
