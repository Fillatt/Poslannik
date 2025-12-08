using Poslannik.Framework.Models;

namespace Poslannik.Client.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> SearchUsersAsync(string searchQuery, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
