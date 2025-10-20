using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на удаление сообщения по идентификатору
    /// </summary>
    public record DeleteMessageRequest
    {
        /// <summary>
        /// Идентификатор удаляемого сообщения
        /// </summary>
        public Guid MessageId { get; init; }
    }
}
