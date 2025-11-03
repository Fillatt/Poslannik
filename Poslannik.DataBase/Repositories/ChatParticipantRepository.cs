using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories
{
    public class ChatParticipantRepository : IChatParticipantRepository
    {
        private readonly ApplicationContext _context;

        public ChatParticipantRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatParticipant>> GetAllAsync()
        {
            var entities = await _context.ChatParticipants
                .Include(cp => cp.Chat)
                .Include(cp => cp.User)
                .ToListAsync();
            return entities.Select(MapToModel);
        }

        public async Task AddAsync(ChatParticipant model)
        {
            var entity = MapToEntity(model);
            await _context.ChatParticipants.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatParticipant model)
        {
            var entity = await _context.ChatParticipants.FindAsync(model.Id);
            if (entity != null)
            {
                MapToEntity(model, entity);
                _context.ChatParticipants.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.ChatParticipants.FindAsync((Guid)(object)id);
            if (entity != null)
            {
                _context.ChatParticipants.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        private ChatParticipantEntity MapToEntity(ChatParticipant model)
        {
            return new ChatParticipantEntity
            {
                Id = model.Id,
                ChatId = model.ChatId,
                UserId = model.UserId,
                UserEncryptedKey = model.UserEncryptedKey
            };
        }

        private void MapToEntity(ChatParticipant model, ChatParticipantEntity entity)
        {
            entity.ChatId = model.ChatId;
            entity.UserId = model.UserId;
            entity.UserEncryptedKey = model.UserEncryptedKey;
        }

        private ChatParticipant MapToModel(ChatParticipantEntity entity)
        {
            return new ChatParticipant
            {
                Id = entity.Id,
                ChatId = entity.ChatId,
                UserId = entity.UserId,
                UserEncryptedKey = entity.UserEncryptedKey
            };
        }
    }
}