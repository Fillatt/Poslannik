using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на получение пользователя по логину
    /// </summary>
    public record GetUserByLoginRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public required string Login { get; init; }
    }
}
