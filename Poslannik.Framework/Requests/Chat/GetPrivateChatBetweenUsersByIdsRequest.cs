using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Chat
{
    public record GetPrivateChatBetweenUsersRequest
    {
        /// <summary>
        /// Идентификатор первого пользователя
        /// </summary>
        public Guid User1Id { get; init; }

        /// <summary>
        /// Идентификатор второго пользователя
        /// </summary>
        public Guid User2Id { get; init; }
    }
}
