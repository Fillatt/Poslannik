using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces;

/// <summary>
/// Интерфейс хаба для управления сообщениями в чатах
/// </summary>
public interface IMessageHub
{
    Task SendMessageAsync(Message message);
}
