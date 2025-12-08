using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces;

/// <summary>
/// Интерфейс хаба для управления пользователями и аутентификацией
/// </summary>
public interface IUserHubRepository
{
    Task<IEnumerable<User>> SearchUsersAsync(string userName);
    Task<User?> GetUserByIdAsync(Guid userId);
}
