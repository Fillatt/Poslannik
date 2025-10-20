using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.ChatParticipant
{
    /// <summary>
    /// Запрос на добавление участника в чат
    /// </summary>
    public record AddParticipantToChatRequest
    {
        /// <summary>
        /// Данные участника чата
        /// </summary>
        public required Models.ChatParticipant Participant { get; init; }
    }
}
