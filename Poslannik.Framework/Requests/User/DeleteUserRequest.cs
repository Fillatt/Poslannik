using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на удаление пользователя по идентификатору
    /// </summary>
    public record DeleteUserRequest
    {
        /// <summary>
        /// Идентификатор удаляемого пользователя
        /// </summary>
        public Guid UserId { get; init; }
    }
}
