using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на получение сообщения по идентификатору
    /// </summary>
    public record GetMessageByIdRequest
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid MessageId { get; init; }
    }
}
