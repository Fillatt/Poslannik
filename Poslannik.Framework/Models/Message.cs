using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models
{
    public record Message
    {
        /// <summary>
        /// Уникальный идентификатор сообщения
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Идентификатор чата, в котором отправлено сообщение
        /// </summary>
        public Guid ChatId { get; init; }

        /// <summary>
        /// Идентификатор пользователя, отправившего сообщение
        /// </summary>
        public Guid SenderId { get; init; }

        /// <summary>
        /// Зашифрованное содержимое сообщения
        /// </summary>
        public required byte[] EncryptedMessage { get; init; }

        /// <summary>
        /// Дата и время отправки сообщения (UTC)
        /// </summary>
        public DateTime SentAt { get; init; }

        /// <summary>
        /// Тип сообщения (Text или PreKeyMessage)
        /// </summary>
        public MessageType MessageType { get; init; }
    }
}
