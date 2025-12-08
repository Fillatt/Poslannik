namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Подписанный предварительный ключ Signal Protocol
    /// </summary>
    public record SignalSignedPreKey
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// ID подписанного предварительного ключа
        /// </summary>
        public int SignedPreKeyId { get; init; }

        /// <summary>
        /// Публичный ключ
        /// </summary>
        public required byte[] PublicKey { get; init; }

        /// <summary>
        /// Приватный ключ
        /// </summary>
        public required byte[] PrivateKey { get; init; }

        /// <summary>
        /// Подпись ключа
        /// </summary>
        public required byte[] Signature { get; init; }

        /// <summary>
        /// Временная метка создания
        /// </summary>
        public DateTime Timestamp { get; init; }
    }
}
