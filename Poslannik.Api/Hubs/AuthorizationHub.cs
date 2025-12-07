using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Requests;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Poslannik.Framework.Services;
using Poslannik.DataBase;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Poslannik.Api.Hubs;

public class AuthorizationHub : Hub, IAuthorizationHub
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthorizationHub(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    ///<Inheritdoc/>
    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request)
    {
        var result = await _userRepository.HasUserByLoginAsync(request.Login);
        if (!result) return new AuthorizationResponse { IsSuccess = false };

        var passwordData = await _userRepository.GetPasswordDataByLoginAsync(request.Login);
        var isVerified = PasswordHasher.VerifyPasswordHash(request.Password, passwordData[PasswordDataType.Hash], passwordData[PasswordDataType.Salt]);
        if (isVerified)
        {
            var userId = await _userRepository.GetUserIdByLoginAsync(request.Login);
            if (userId == null) return new AuthorizationResponse { IsSuccess = false };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Login),
                new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString())
            };

            var jwtSecretKey = _configuration["Jwt:SecretKey"]!;
            var jwtIssuer = _configuration["Jwt:Issuer"]!;
            var jwtAudience = _configuration["Jwt:Audience"]!;
            var jwtExpiration = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtExpiration),
                signingCredentials: credentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AuthorizationResponse { IsSuccess = true, Token = encodedJwt };
        }
        else return new AuthorizationResponse { IsSuccess = false };
    }
}
