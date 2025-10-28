using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories
{
    public interface IUserRepository : IRepository<UserEntity, User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByConnectionIdAsync(string connectionId);
        Task UpdateConnectionIdAsync(Guid userId, string connectionId);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }

    public interface IChatRepository : IRepository<ChatEntity, Chat>
    {
        Task<Chat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id);
        Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId);
        Task<IEnumerable<Chat>> GetGroupChatsByAdminAsync(Guid adminId);
        Task<bool> IsUserInChatAsync(Guid chatId, Guid userId);
        Task<Chat?> GetByIdAsync(Guid id);
    }

    public interface IChatParticipantRepository : IRepository<ChatParticipantEntity, ChatParticipant>
    {
        Task<ChatParticipant?> GetByChatAndUserAsync(Guid chatId, Guid userId);
        Task<IEnumerable<ChatParticipant>> GetByChatIdAsync(Guid chatId);
        Task<IEnumerable<ChatParticipant>> GetByUserIdAsync(Guid userId);
        Task RemoveUserFromChatAsync(Guid chatId, Guid userId);
    }

    public interface IMessageRepository : IRepository<MessageEntity, Message>
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId);
        Task<IEnumerable<Message>> GetMessagesBySenderAsync(Guid senderId);
        Task<Message?> GetLastMessageByChatAsync(Guid chatId);
        Task<IEnumerable<Message>> GetMessagesByChatWithPaginationAsync(Guid chatId, int page, int pageSize);
    }
}