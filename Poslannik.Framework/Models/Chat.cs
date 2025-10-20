using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Модель чата для обмена сообщениями между пользователями
    /// </summary>
    public record Chat
    {
        /// <summary>
        /// Уникальный идентификатор чата
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Тип чата
        /// </summary>
        public ChatType ChatType { get; init; }

        /// <summary>
        /// Идентификатор первого пользователя (для приватных чатов)
        /// </summary>
        public Guid? User1Id { get; init; }

        /// <summary>
        /// Идентификатор второго пользователя (для приватных чатов)
        /// </summary>
        public Guid? User2Id { get; init; }

        /// <summary>
        /// Название чата (для групповых чатов)
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Зашифрованный групповой ключ (для групповых чатов)
        /// </summary>
        public byte[]? EncryptedGroupKey { get; init; }

        /// <summary>
        /// Идентификатор администратора группы (для групповых чатов)
        /// </summary>
        public Guid? AdminId { get; init; }
    }
}
