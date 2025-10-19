using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;
using Poslannik.Framework.Requests.Chat;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для системы обмена сообщениями в реальном времени
    /// </summary>
    public interface IChatHub
    {
        /// <summary>
        /// Получает чат по идентификатору
        /// </summary>
        Task<Chat> GetChatById(ByIdRequest request);

        /// <summary>
        /// Получает все чаты пользователя
        /// </summary>
        Task<List<Chat>> GetUserChatsById(ByIdRequest request);

        /// <summary>
        /// Получает приватный чат между двумя пользователями (Зачем вообще он нужен!!!!)
        /// </summary>
        Task<Chat> GetPrivateChatBetweenUsersByIds(GetPrivateChatBetweenUsersRequest request);

        /// <summary>
        /// Создает новый чат
        /// </summary>
        Task<Chat> CreateChat(CreateChatRequest request);

        /// <summary>
        /// Удаляет чат по идентификатору
        /// </summary>
        Task DeleteChatById(DeleteChatRequest request);
    }
}
