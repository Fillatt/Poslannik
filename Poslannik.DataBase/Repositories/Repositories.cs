using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories
{
    public class Repositories : BaseRepository<UserEntity, User>, IUserRepository
    {
        public Repositories(ApplicationContext context) : base(context) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(u => u.Login == username);
            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<User?> GetByConnectionIdAsync(string connectionId)
        {
            // Если в будущем добавится ConnectionId в UserEntity
            var entity = await _dbSet.FirstOrDefaultAsync(u => EF.Property<string>(u, "ConnectionId") == connectionId);
            return entity != null ? MapToModel(entity) : null;
        }

        public async Task UpdateConnectionIdAsync(Guid userId, string connectionId)
        {
            var entity = await _dbSet.FindAsync(userId);
            if (entity != null)
            {
                // Реализация при добавлении ConnectionId
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            var entities = await _dbSet
                .Where(u => u.Login.Contains(searchTerm) ||
                           (u.UserName != null && u.UserName.Contains(searchTerm)) ||
                           (u.GroupUser != null && u.GroupUser.Contains(searchTerm)))
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null ? MapToModel(entity) : null;
        }

        protected override User MapToModel(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                UserName = entity.UserName,
                GroupUser = entity.GroupUser,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt,
                PublicKey = entity.PublicKey
            };
        }

        protected override UserEntity MapToEntity(User model)
        {
            return new UserEntity
            {
                Id = model.Id,
                Login = model.Login,
                UserName = model.UserName,
                GroupUser = model.GroupUser,
                PasswordHash = model.PasswordHash,
                PasswordSalt = model.PasswordSalt,
                PublicKey = model.PublicKey
            };
        }
    }

    public class ChatRepository : BaseRepository<ChatEntity, Chat>, IChatRepository
    {
        public ChatRepository(ApplicationContext context) : base(context) { }

        public async Task<Chat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id)
        {
            var entity = await _dbSet
                .FirstOrDefaultAsync(c => c.ChatType == (int)ChatType.Private &&
                    ((c.User1Id == user1Id && c.User2Id == user2Id) ||
                     (c.User1Id == user2Id && c.User2Id == user1Id)));

            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId)
        {
            var entities = await _dbSet
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .Include(c => c.Participants)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<IEnumerable<Chat>> GetGroupChatsByAdminAsync(Guid adminId)
        {
            var entities = await _dbSet
                .Where(c => c.ChatType == (int)ChatType.Group && c.AdminId == adminId)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<bool> IsUserInChatAsync(Guid chatId, Guid userId)
        {
            return await _dbSet
                .AnyAsync(c => c.Id == chatId && c.Participants.Any(p => p.UserId == userId));
        }

        public async Task<Chat?> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null ? MapToModel(entity) : null;
        }

        protected override Chat MapToModel(ChatEntity entity)
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

        protected override ChatEntity MapToEntity(Chat model)
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
    }

    public class ChatParticipantRepository : BaseRepository<ChatParticipantEntity, ChatParticipant>, IChatParticipantRepository
    {
        public ChatParticipantRepository(ApplicationContext context) : base(context) { }

        public async Task<ChatParticipant?> GetByChatAndUserAsync(Guid chatId, Guid userId)
        {
            var entity = await _dbSet
                .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<IEnumerable<ChatParticipant>> GetByChatIdAsync(Guid chatId)
        {
            var entities = await _dbSet
                .Where(cp => cp.ChatId == chatId)
                .Include(cp => cp.User)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<IEnumerable<ChatParticipant>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _dbSet
                .Where(cp => cp.UserId == userId)
                .Include(cp => cp.Chat)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
        {
            var entity = await GetByChatAndUserAsync(chatId, userId);
            if (entity != null)
            {
                var entityToDelete = MapToEntity(entity);
                _dbSet.Remove(entityToDelete);
                await _context.SaveChangesAsync();
            }
        }

        protected override ChatParticipant MapToModel(ChatParticipantEntity entity)
        {
            return new ChatParticipant
            {
                Id = entity.Id,
                ChatId = entity.ChatId,
                UserId = entity.UserId,
                UserEncryptedKey = entity.UserEncryptedKey
            };
        }

        protected override ChatParticipantEntity MapToEntity(ChatParticipant model)
        {
            return new ChatParticipantEntity
            {
                Id = model.Id,
                ChatId = model.ChatId,
                UserId = model.UserId,
                UserEncryptedKey = model.UserEncryptedKey
            };
        }
    }

    public class MessageRepository : BaseRepository<MessageEntity, Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId)
        {
            var entities = await _dbSet
                .Where(m => m.ChatId == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.Id)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<IEnumerable<Message>> GetMessagesBySenderAsync(Guid senderId)
        {
            var entities = await _dbSet
                .Where(m => m.SenderId == senderId)
                .Include(m => m.Chat)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        public async Task<Message?> GetLastMessageByChatAsync(Guid chatId)
        {
            var entity = await _dbSet
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();

            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatWithPaginationAsync(Guid chatId, int page, int pageSize)
        {
            var entities = await _dbSet
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Sender)
                .ToListAsync();

            return entities.Select(MapToModel);
        }

        protected override Message MapToModel(MessageEntity entity)
        {
            return new Message
            {
                Id = entity.Id,
                ChatId = entity.ChatId,
                SenderId = entity.SenderId,
                EncryptedMessage = entity.EncryptedMessage
            };
        }

        protected override MessageEntity MapToEntity(Message model)
        {
            return new MessageEntity
            {
                Id = model.Id,
                ChatId = model.ChatId,
                SenderId = model.SenderId,
                EncryptedMessage = model.EncryptedMessage
            };
        }
    }
}