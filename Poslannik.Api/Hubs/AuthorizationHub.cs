using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Requests;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Poslannik.Framework.Services;
using Poslannik.DataBase;

namespace Poslannik.Api.Hubs;

public class AuthorizationHub (IUserRepository userRepository) : Hub, IAuthorizationHub
{
    ///<Inheritdoc/>
    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request)
    {
        var result = await userRepository.HasUserByLoginAsync(request.Login);
        if (!result) return new AuthorizationResponse { IsSuccess = false };

        var passwordData = await userRepository.GetPasswordDataByLoginAsync(request.Login);
        var isVerified = PasswordHasher.VerifyPasswordHash(request.Password, passwordData[PasswordDataType.Hash], passwordData[PasswordDataType.Salt]);
        if (isVerified)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, request.Login) };
            var jwt = new JwtSecurityToken(claims: claims);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AuthorizationResponse { IsSuccess = true, Token = encodedJwt };
        }
        else return new AuthorizationResponse { IsSuccess = false };
    }
}
