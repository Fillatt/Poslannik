using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.ChatParticipant
{
    /// <summary>
    /// Запрос на удаление всех участников из чата
    /// </summary>
    public record RemoveAllParticipantsFromChatRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }
    }
}
