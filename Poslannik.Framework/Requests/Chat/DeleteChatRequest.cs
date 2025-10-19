using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Chat
{
    public record DeleteChatRequest
    {
        /// <summary>
        /// Идентификатор удаляемого чата
        /// </summary>
        public Guid ChatId { get; init; }
    }
}
