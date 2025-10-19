using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на получение пользователя по идентификатору
    /// </summary>
    public record GetUserByIdRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; init; }
    }
}
