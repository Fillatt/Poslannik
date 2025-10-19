using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Requests.Chat
{
    /// <summary>
    /// Запрос на создание нового чата
    /// </summary>
    public record CreateChatRequest
    {
        /// <summary>
        /// Данные создаваемого чата
        /// </summary>
        public required Models.Chat Chat { get; init; }
    }
}
