using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления участниками чатов
    /// </summary>
    public interface IChatParticipantHubRepository
    {
        Task<IEnumerable<ChatParticipant>> GetAllAsync();
        Task AddAsync(ChatParticipant model);
        Task UpdateAsync(ChatParticipant model);
        Task DeleteAsync(long id);

    }
}
