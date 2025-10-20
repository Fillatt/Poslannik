using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;
using Poslannik.Framework.Requests.Message;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления сообщениями в чатах
    /// </summary>
    public interface IMessageHub
    {
        /// <summary>
        /// Получает сообщение по идентификатору
        /// </summary>
        Task<Message> GetMessageById(GetMessageByIdRequest request);

        /// <summary>
        /// Получает все сообщения чата
        /// </summary>
        Task<List<Message>> GetChatMessages(GetChatMessagesRequest request);

        /// <summary>
        /// Получает сообщения чата с пагинацией
        /// </summary>
        Task<List<Message>> GetChatMessagesPaged(GetChatMessagesPagedRequest request);

        /// <summary>
        /// Получает все сообщения пользователя
        /// </summary>
        Task<List<Message>> GetUserMessages(GetUserMessagesRequest request);

        /// <summary>
        /// Создает новое сообщение
        /// </summary>
        Task<Message> CreateMessage(CreateMessageRequest request);

        /// <summary>
        /// Удаляет сообщение по идентификатору
        /// </summary>
        Task DeleteMessage(DeleteMessageRequest request);

        /// <summary>
        /// Удаляет все сообщения чата
        /// </summary>
        Task DeleteChatMessages(DeleteChatMessagesRequest request);
    }
}
