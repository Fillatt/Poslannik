using Poslannik.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity, User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByConnectionIdAsync(string connectionId);
        Task UpdateConnectionIdAsync(Guid userId, string connectionId);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<User?> GetByIdAsync(Guid id);
    }
}
