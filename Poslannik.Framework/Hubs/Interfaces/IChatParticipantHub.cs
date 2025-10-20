using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Models;
using Poslannik.Framework.Requests.ChatParticipant;

namespace Poslannik.Framework.Hubs.Interfaces
{
    /// <summary>
    /// Интерфейс хаба для управления участниками чатов
    /// </summary>
    public interface IChatParticipantHub
    {
        /// <summary>
        /// Получает участника чата по идентификаторам чата и пользователя
        /// </summary>
        Task<ChatParticipant> GetChatParticipant(GetChatParticipantRequest request);

        /// <summary>
        /// Получает всех участников чата
        /// </summary>
        Task<List<ChatParticipant>> GetChatParticipants(GetChatParticipantsRequest request);

        /// <summary>
        /// Добавляет участника в чат
        /// </summary>
        Task AddParticipantToChat(AddParticipantToChatRequest request);

        /// <summary>
        /// Удаляет участника из чата
        /// </summary>
        Task RemoveParticipantFromChat(RemoveParticipantFromChatRequest request);

        /// <summary>
        /// Удаляет всех участников из чата
        /// </summary>
        Task RemoveAllParticipantsFromChat(RemoveAllParticipantsFromChatRequest request);

    }
}
