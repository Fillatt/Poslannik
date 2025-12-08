using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления сообщениями в чатах
    /// </summary>
    public interface IMessageHubRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task AddAsync(Message model);
        Task UpdateAsync(Message model);
        Task DeleteAsync(long id);

        /// <summary>
        /// Отправка сообщения в чат
        /// </summary>
        Task<Message?> SendMessageAsync(Guid chatId, string messageText);

        /// <summary>
        /// Получение всех сообщений чата
        /// </summary>
        Task<IEnumerable<Message>> GetChatMessagesAsync(Guid chatId);
    }
}
