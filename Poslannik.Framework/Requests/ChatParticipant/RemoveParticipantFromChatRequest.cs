using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.ChatParticipant
{
    /// <summary>
    /// Запрос на удаление участника из чата
    /// </summary>
    public record RemoveParticipantFromChatRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }
    }
}
