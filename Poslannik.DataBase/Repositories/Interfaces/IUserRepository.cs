using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> HasUserByLoginAsync(string login);

    Task<Dictionary<PasswordDataType, byte[]>?> GetPasswordDataByLoginAsync(string login);
}
