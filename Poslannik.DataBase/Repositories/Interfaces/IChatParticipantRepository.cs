using Poslannik.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IChatParticipantRepository : IRepository<ChatParticipantEntity, ChatParticipant>
    {
        Task<ChatParticipant?> GetByChatAndUserAsync(Guid chatId, Guid userId);
        Task<IEnumerable<ChatParticipant>> GetByChatIdAsync(Guid chatId);
        Task<IEnumerable<ChatParticipant>> GetByUserIdAsync(Guid userId);
        Task RemoveUserFromChatAsync(Guid chatId, Guid userId);
    }
}
