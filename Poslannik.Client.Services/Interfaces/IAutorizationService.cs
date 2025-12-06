using Poslannik.Framework.Responses;

namespace Poslannik.Client.Services.Interfaces;

public interface IAutorizationService
{
    string? JwtToken { get; set; }

    bool IsAuthorizated { get; set; }

    Task<AuthorizationResponse> AuthorizeAsync(string login, string password, CancellationToken cancellationToken);

    Task LogoutAsync(CancellationToken cancellationToken);
}
