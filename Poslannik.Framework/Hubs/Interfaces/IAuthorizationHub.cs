using Poslannik.Framework.Requests;
using Poslannik.Framework.Responses;

namespace Poslannik.Framework.Hubs.Interfaces;

public interface IAuthorizationHub
{
    /// <summary>
    /// Верификация пароля
    /// </summary>
    /// <param name="request">Данные запроса для верификации</param>
    public Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request);
}
