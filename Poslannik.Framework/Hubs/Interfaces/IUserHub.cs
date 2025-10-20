using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;
using Poslannik.Framework.Requests.User;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления пользователями и аутентификацией
    /// </summary>
    public interface IUserHub
    {
        /// <summary>
        /// Получает пользователя по идентификатору
        /// </summary>
        Task<User> GetUserById(GetUserByIdRequest request);

        /// <summary>
        /// Получает пользователя по логину
        /// </summary>
        Task<User> GetUserByLogin(GetUserByLoginRequest request);

        /// <summary>
        /// Получает список пользователей по списку идентификаторов
        /// </summary>
        Task<List<User>> GetUsersByIds(GetUsersByIdsRequest request);

        /// <summary>
        /// Создает нового пользователя
        /// </summary>
        Task<User> CreateUser(CreateUserRequest request);

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        Task UpdateUser(UpdateUserRequest request);

        /// <summary>
        /// Удаляет пользователя по идентификатору
        /// </summary>
        Task DeleteUser(DeleteUserRequest request);
    }
}
