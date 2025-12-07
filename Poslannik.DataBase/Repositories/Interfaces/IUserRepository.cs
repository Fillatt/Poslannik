using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> HasUserByLoginAsync(string login);

    Task<Dictionary<PasswordDataType, byte[]>?> GetPasswordDataByLoginAsync(string login);

    Task<Guid?> GetUserIdByLoginAsync(string login);

    Task<IEnumerable<User>> SearchUsersByNameAsync(string searchQuery);
}
