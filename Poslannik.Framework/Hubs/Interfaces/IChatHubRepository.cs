using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;


namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для системы обмена сообщениями в реальном времени
    /// </summary>
    public interface IChatHubRepository
    {
        /// <summary>
        /// Получает все чаты пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Список чатов пользователя</returns>
        Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId);

        /// <summary>
        /// Создает новый чат
        /// </summary>
        /// <param name="chat">Данные чата</param>
        /// <returns>Созданный чат с идентификатором</returns>
        Task<Chat> CreateChatAsync(Chat chat);

        /// <summary>
        /// Уведомляет участников чата о событии
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="chat">Данные чата для отправки</param>
        Task NotifyChatParticipantsAsync(Guid chatId, Chat chat);
    }
}
