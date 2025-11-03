using Poslannik.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<MessageEntity, Message>
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId);
        Task<IEnumerable<Message>> GetMessagesBySenderAsync(Guid senderId);
        Task<Message?> GetLastMessageByChatAsync(Guid chatId);
        Task<IEnumerable<Message>> GetMessagesByChatWithPaginationAsync(Guid chatId, int page, int pageSize);
    }
}
