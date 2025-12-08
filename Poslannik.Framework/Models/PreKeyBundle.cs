namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Набор ключей для установки Signal сессии (DTO)
    /// </summary>
    public record PreKeyBundle
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Регистрационный ID
        /// </summary>
        public int RegistrationId { get; init; }

        /// <summary>
        /// ID устройства
        /// </summary>
        public int DeviceId { get; init; }

        /// <summary>
        /// ID предварительного ключа
        /// </summary>
        public int PreKeyId { get; init; }

        /// <summary>
        /// Публичный предварительный ключ
        /// </summary>
        public required byte[] PreKeyPublic { get; init; }

        /// <summary>
        /// ID подписанного предварительного ключа
        /// </summary>
        public int SignedPreKeyId { get; init; }

        /// <summary>
        /// Публичный подписанный предварительный ключ
        /// </summary>
        public required byte[] SignedPreKeyPublic { get; init; }

        /// <summary>
        /// Подпись подписанного предварительного ключа
        /// </summary>
        public required byte[] SignedPreKeySignature { get; init; }

        /// <summary>
        /// Публичный ключ идентификации
        /// </summary>
        public required byte[] IdentityKey { get; init; }
    }
}
