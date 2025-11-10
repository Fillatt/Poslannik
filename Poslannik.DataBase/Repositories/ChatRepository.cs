using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationContext _context;

        public ChatRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllAsync()
        {
            var entities = await _context.Chats
                .Include(c => c.Participants)
                .Include(c => c.Messages)
                .ToListAsync();
            return entities.Select(MapToModel);
        }

        public async Task AddAsync(Chat model)
        {
            var entity = MapToEntity(model);
            await _context.Chats.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chat model)
        {
            var entity = await _context.Chats.FindAsync(model.Id);
            if (entity != null)
            {
                MapToEntity(model, entity);
                _context.Chats.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Chats.FindAsync((Guid)(object)id);
            if (entity != null)
            {
                _context.Chats.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        private ChatEntity MapToEntity(Chat model)
        {
            return new ChatEntity
            {
                Id = model.Id,
                ChatType = (int)model.ChatType,
                User1Id = model.User1Id,
                User2Id = model.User2Id,
                Name = model.Name,
                EncryptedGroupKey = model.EncryptedGroupKey,
                AdminId = model.AdminId
            };
        }

        private void MapToEntity(Chat model, ChatEntity entity)
        {
            entity.ChatType = (int)model.ChatType;
            entity.User1Id = model.User1Id;
            entity.User2Id = model.User2Id;
            entity.Name = model.Name;
            entity.EncryptedGroupKey = model.EncryptedGroupKey;
            entity.AdminId = model.AdminId;
        }

        private Chat MapToModel(ChatEntity entity)
        {
            return new Chat
            {
                Id = entity.Id,
                ChatType = (ChatType)entity.ChatType,
                User1Id = entity.User1Id,
                User2Id = entity.User2Id,
                Name = entity.Name,
                EncryptedGroupKey = entity.EncryptedGroupKey,
                AdminId = entity.AdminId
            };
        }
    }
}