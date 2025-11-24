namespace Poslannik.Framework.Responses
{
    /// <summary>Ответ авторизации пользователя.</summary>
    public record AuthorizationResponse
    {
        /// <summary>Флаг успеха авторизации.</summary>
        public bool IsSuccess { get; init; }

        /// <summary>JWT-токен.</summary>
        public string Token { get; init; } = string.Empty;
    }
}
