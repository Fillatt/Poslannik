using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.ChatParticipant
{
    /// <summary>
    /// Запрос на получение всех участников чата
    /// </summary>
    public record GetChatParticipantsRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }
    }
}
