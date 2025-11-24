using Poslannik.Api.Services;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Requests;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Api.Hubs
{
    public class AuthorizationHub (IUserRepository userRepository) : IAuthorizationHub
    {
        ///<Inheritdoc/>
         public async Task<bool> VerifyAsync(VerifyRequest request)
        {
            var user = await userRepository.GetUserByLoginAsync(request.Login);
            if (user == null) return false;

            return PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
        }
    }
}
