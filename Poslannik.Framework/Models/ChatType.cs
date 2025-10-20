using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models
{
    /// <summary>
    /// Тип чата
    /// </summary>
    public enum ChatType
    {
        /// <summary>
        /// Приватный чат между двумя пользователями
        /// </summary>
        Private = 1,

        /// <summary>
        /// Групповой чат с несколькими участниками
        /// </summary>
        Group = 2
    }
}
