using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на получение сообщений чата с пагинацией
    /// </summary>
    public record GetChatMessagesPagedRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }

        /// <summary>
        /// Количество пропускаемых сообщений
        /// </summary>
        public int Skip { get; init; }

        /// <summary>
        /// Количество получаемых сообщений
        /// </summary>
        public int Take { get; init; }
    }
}
