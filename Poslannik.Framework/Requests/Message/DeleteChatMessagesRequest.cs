using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на удаление всех сообщений чата
    /// </summary>
    public record DeleteChatMessagesRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }
    }
}
