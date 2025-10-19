using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на получение списка пользователей по идентификаторам
    /// </summary>
    public record GetUsersByIdsRequest
    {
        /// <summary>
        /// Список идентификаторов пользователей
        /// </summary>
        public required List<Guid> UserIds { get; init; }
    }
}
