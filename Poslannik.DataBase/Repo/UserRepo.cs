using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationContext _dbContext;

        public UserRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получает пользователя по идентификатору
        /// </summary>
        public Task<User?> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        /// <summary>
        /// Получает идентификатор пользователя по логину
        /// </summary>
        public Task<Guid?> GetUserIdByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => u.Login == login)
                .Select(u => (Guid?)u.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Получает пользователя по логину
        /// </summary>
        public Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        }

        /// <summary>
        /// Получает список пользователей по списку идентификаторов
        /// </summary>
        public Task<List<User>> GetUsersByIds(List<Guid> userIds, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Создает нового пользователя
        /// </summary>
        public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет пользователя по идентификатору
        /// </summary>
        public async Task DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Проверяет существование пользователя с указанным логином
        /// </summary>
        public Task<bool> UserExists(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .AnyAsync(u => u.Login == login, cancellationToken);
        }
    }
}