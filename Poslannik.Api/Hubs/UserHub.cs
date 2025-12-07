using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Api.Hubs;

[Authorize]
public class UserHub : Hub, IUserHubRepository
{
    private readonly IUserRepository _userRepository;

    public UserHub(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Поиск пользователей по имени
    /// </summary>
    /// <param name="searchQuery">Поисковый запрос</param>
    /// <returns>Список найденных пользователей</returns>
    public async Task<IEnumerable<User>> SearchUsersAsync(string userName)
    {
        return await _userRepository.SearchUsersByNameAsync(userName);
    }
}
