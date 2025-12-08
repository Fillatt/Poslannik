using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories;

public class SignalSessionRepository : ISignalSessionRepository
{
    private readonly ApplicationContext _context;

    public SignalSessionRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SignalSession?> GetSessionAsync(Guid userId, Guid recipientId, int deviceId)
    {
        var entity = await _context.SignalSessions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RecipientId == recipientId && x.DeviceId == deviceId);

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task SaveSessionAsync(SignalSession session)
    {
        var existing = await _context.SignalSessions
            .FirstOrDefaultAsync(x => x.UserId == session.UserId &&
                                     x.RecipientId == session.RecipientId &&
                                     x.DeviceId == session.DeviceId);

        if (existing != null)
        {
            existing.SessionState = session.SessionState;
            existing.LastUpdated = DateTime.UtcNow;
        }
        else
        {
            var entity = MapToEntity(session);
            await _context.SignalSessions.AddAsync(entity);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSessionAsync(Guid userId, Guid recipientId, int deviceId)
    {
        var entity = await _context.SignalSessions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RecipientId == recipientId && x.DeviceId == deviceId);

        if (entity != null)
        {
            _context.SignalSessions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid recipientId, int deviceId)
    {
        return await _context.SignalSessions
            .AnyAsync(x => x.UserId == userId && x.RecipientId == recipientId && x.DeviceId == deviceId);
    }

    private SignalSessionEntity MapToEntity(SignalSession model)
    {
        return new SignalSessionEntity
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            RecipientId = model.RecipientId,
            DeviceId = model.DeviceId,
            SessionState = model.SessionState,
            LastUpdated = model.LastUpdated
        };
    }

    private SignalSession MapToModel(SignalSessionEntity entity)
    {
        return new SignalSession
        {
            UserId = entity.UserId,
            RecipientId = entity.RecipientId,
            DeviceId = entity.DeviceId,
            SessionState = entity.SessionState,
            LastUpdated = entity.LastUpdated
        };
    }
}
