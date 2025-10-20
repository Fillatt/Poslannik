using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IUserRepo
    {
        /// <summary>
        /// Получает пользователя по идентификатору
        /// </summary>
        Task<User?> GetUserById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает идентификатор пользователя по логину
        /// </summary>
        Task<Guid?> GetUserIdByLogin(string login, CancellationToken cancellationToken);

        /// <summary>
        /// Получает пользователя по логину
        /// </summary>
        Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken);

        /// <summary>
        /// Получает список пользователей по списку идентификаторов
        /// </summary>
        Task<List<User>> GetUsersByIds(List<Guid> userIds, CancellationToken cancellationToken);

        /// <summary>
        /// Создает нового пользователя
        /// </summary>
        Task<User> CreateUser(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        Task UpdateUser(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет пользователя по идентификатору
        /// </summary>
        Task DeleteUser(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет существование пользователя с указанным логином
        /// </summary>
        Task<bool> UserExists(string login, CancellationToken cancellationToken);
    }
}