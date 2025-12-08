namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Сессия шифрования Signal Protocol между двумя пользователями
    /// </summary>
    public record SignalSession
    {
        /// <summary>
        /// Идентификатор пользователя, владеющего сессией
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Идентификатор собеседника
        /// </summary>
        public Guid RecipientId { get; init; }

        /// <summary>
        /// ID устройства (для поддержки нескольких устройств)
        /// </summary>
        public int DeviceId { get; init; }

        /// <summary>
        /// Сериализованное состояние сессии
        /// </summary>
        public required byte[] SessionState { get; init; }

        /// <summary>
        /// Временная метка последнего обновления
        /// </summary>
        public DateTime LastUpdated { get; init; }
    }
}
