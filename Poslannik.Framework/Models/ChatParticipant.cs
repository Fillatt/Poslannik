using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Представляет участника чата с информацией о его участии и правах доступа
    /// </summary>
    public record ChatParticipant
    {
        /// <summary>
        /// Уникальный идентификатор записи участника чата
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор чата, в котором состоит участник
        /// </summary>
        public Guid ChatId { get; set; }

        /// <summary>
        /// Идентификатор пользователя-участника чата
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Зашифрованный ключ чата для данного пользователя (для групповых чатов)
        /// </summary>
        public byte[]? UserEncryptedKey { get; set; }
    }
}
