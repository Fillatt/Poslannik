using Poslannik.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IChatRepository : IRepository<ChatEntity, Chat>
    {
        Task<Chat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id);
        Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId);
        Task<IEnumerable<Chat>> GetGroupChatsByAdminAsync(Guid adminId);
        Task<bool> IsUserInChatAsync(Guid chatId, Guid userId);
        Task<Chat?> GetByIdAsync(Guid id);
    }
}
