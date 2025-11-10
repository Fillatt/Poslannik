using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;


namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для системы обмена сообщениями в реальном времени
    /// </summary>
    public interface IChatHubRepository
    {
        Task<IEnumerable<Chat>> GetAllAsync();
        Task AddAsync(Chat model);
        Task UpdateAsync(Chat model);
        Task DeleteAsync(long id);
    }
}
