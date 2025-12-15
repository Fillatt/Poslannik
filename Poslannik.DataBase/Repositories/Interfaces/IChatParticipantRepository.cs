using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IChatParticipantRepository : IRepository<ChatParticipantEntity, ChatParticipant>
    {
        /// <summary>
        /// Получает всех участников чата по ID чата
        /// </summary>
        Task<IEnumerable<ChatParticipant>> GetByChatIdAsync(Guid chatId);

        /// <summary>
        /// Удаляет конкретного участника из чата
        /// </summary>
        Task DeleteByUserAndChatAsync(Guid chatId, Guid userId);
    }
}
