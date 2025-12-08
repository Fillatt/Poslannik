namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Одноразовый предварительный ключ Signal Protocol
    /// </summary>
    public record SignalPreKey
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// ID предварительного ключа
        /// </summary>
        public int PreKeyId { get; init; }

        /// <summary>
        /// Публичный ключ
        /// </summary>
        public required byte[] PublicKey { get; init; }

        /// <summary>
        /// Приватный ключ
        /// </summary>
        public required byte[] PrivateKey { get; init; }

        /// <summary>
        /// Флаг использования (одноразовый)
        /// </summary>
        public bool IsUsed { get; init; }
    }
}
