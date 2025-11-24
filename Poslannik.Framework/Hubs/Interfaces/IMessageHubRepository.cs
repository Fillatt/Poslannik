using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления сообщениями в чатах
    /// </summary>
    public interface IMessageHubRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task AddAsync(Message model);
        Task UpdateAsync(Message model);
        Task DeleteAsync(long id);
    }
}
