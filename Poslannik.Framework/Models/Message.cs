using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.Framework.Models;

public record Message
{
    /// <summary>
    /// Уникальный идентификатор сообщения
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор чата, в котором отправлено сообщение
    /// </summary>
    public Guid ChatId { get; init; }

    /// <summary>
    /// Идентификатор пользователя, отправившего сообщение
    /// </summary>
    public Guid SenderId { get; init; }

    public DateTime DateTime { get; set; }

    /// <summary>
    /// Содержимое сообщения
    /// </summary>
    public required string Data { get; init; }
}
