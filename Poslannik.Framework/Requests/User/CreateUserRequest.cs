using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на создание нового пользователя
    /// </summary>
    public record CreateUserRequest
    {
        /// <summary>
        /// Данные создаваемого пользователя
        /// </summary>
        public required Models.User User { get; init; }
    }
}
