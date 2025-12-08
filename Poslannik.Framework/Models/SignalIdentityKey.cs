namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Идентификационный ключ Signal Protocol
    /// </summary>
    public record SignalIdentityKey
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Публичный ключ идентификации
        /// </summary>
        public required byte[] PublicKey { get; init; }

        /// <summary>
        /// Приватный ключ идентификации
        /// </summary>
        public required byte[] PrivateKey { get; init; }

        /// <summary>
        /// Регистрационный ID
        /// </summary>
        public int RegistrationId { get; init; }
    }
}
