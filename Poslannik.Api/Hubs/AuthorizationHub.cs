using Poslannik.Api.Services;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Requests;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Poslannik.Api.Hubs;

public class AuthorizationHub (IUserRepository userRepository) : Hub, IAuthorizationHub
{
    ///<Inheritdoc/>
    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request)
    {
        var user = await userRepository.GetUserByLoginAsync(request.Login);
        if (user == null) return new AuthorizationResponse { IsSuccess = false };

        var isVerified = PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
        if (isVerified)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
            var jwt = new JwtSecurityToken(claims: claims);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AuthorizationResponse { IsSuccess = true, Token = encodedJwt };
        }
        else return new AuthorizationResponse { IsSuccess = false };
    }
}
