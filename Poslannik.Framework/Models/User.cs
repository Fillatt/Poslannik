using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Представляет пользователя мессенджера
    /// </summary>
    public record User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Логин для входа в систему
        /// </summary>
        public required string Login { get; set; }

        /// <summary>Пароль.</summary>
        public required string Password { get; set; }

        /// <summary>
        /// Отображаемое имя пользователя
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Группа пользователя (для системных целей)
        /// </summary>
        public string? GroupUser { get; set; }
    }
}
