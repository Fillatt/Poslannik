using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Requests.Chat
{
    /// <summary>
    /// Запрос на получение чата по идентификатору
    /// </summary>
    public record ByIdRequest
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; init; }
    
    }
}
