using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на получение всех сообщений чата
    /// </summary>
    public record GetChatMessagesRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }
    }
}
