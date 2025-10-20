using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.User
{
    /// <summary>
    /// Запрос на обновление данных пользователя
    /// </summary>
    public record UpdateUserRequest
    {
        /// <summary>
        /// Обновленные данные пользователя
        /// </summary>
        public required Models.User User { get; init; }
    }
}
