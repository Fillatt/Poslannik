using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationContext _context;

    public MessageRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        var entities = await _context.Messages
            .Include(m => m.Chat)
            .Include(m => m.Sender)
            .ToListAsync();
        return entities.Select(MapToModel);
    }

    public async Task AddAsync(Message model)
    {
        var entity = MapToEntity(model);
        await _context.Messages.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message model)
    {
        var entity = await _context.Messages.FindAsync(model.Id);
        if (entity != null)
        {
            MapToEntity(model, entity);
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _context.Messages.FindAsync((Guid)(object)id);
        if (entity != null)
        {
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyList<Message>> GetAllMessageByChatId(Guid chatId)
    {
        var entities = _context.Messages.Where(x => x.ChatId == chatId);
        var messages = new List<Message>();
        foreach (var entity in entities) messages.Add(MapToModel(entity));
        return messages;
    }

    private MessageEntity MapToEntity(Message model)
    {
        return new MessageEntity
        {
            Id = model.Id,
            ChatId = model.ChatId,
            SenderId = model.SenderId,
            DateTime = model.DateTime,
            Data = model.Data,
        };
    }

    private void MapToEntity(Message model, MessageEntity entity)
    {
        entity.ChatId = model.ChatId;
        entity.SenderId = model.SenderId;
        entity.Data = model.Data;
        entity.DateTime = model.DateTime;
    }

    private Message MapToModel(MessageEntity entity)
    {
        return new Message
        {
            Id = entity.Id,
            ChatId = entity.ChatId,
            SenderId = entity.SenderId,
            Data = entity.Data,
            DateTime = entity.DateTime
        };
    }
}