using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Message
{
    /// <summary>
    /// Запрос на создание нового сообщения
    /// </summary>
    public record CreateMessageRequest
    {
        /// <summary>
        /// Данные создаваемого сообщения
        /// </summary>
        public required Models.Message Message { get; init; }
    }
}
