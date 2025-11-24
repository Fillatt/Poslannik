using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления пользователями и аутентификацией
    /// </summary>
    public interface IUserHubRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User model);
        Task UpdateAsync(User model);
        Task DeleteAsync(long id);
    }
}
