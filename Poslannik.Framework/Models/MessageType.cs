namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Обычное текстовое сообщение
        /// </summary>
        Text = 0,

        /// <summary>
        /// Сообщение с PreKey (для инициализации сессии)
        /// </summary>
        PreKeyMessage = 1,

        /// <summary>
        /// Системное сообщение (уведомление о событиях в чате)
        /// </summary>
        System = 2
    }
}
