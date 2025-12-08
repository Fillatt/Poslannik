using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IChatRepository : IRepository<ChatEntity, Chat>
    {
        /// <summary>
        /// Получает чат по идентификатору
        /// </summary>
        Task<Chat?> GetByIdAsync(Guid chatId);

        /// <summary>
        /// Получает все чаты пользователя по его идентификатору
        /// </summary>
        Task<IEnumerable<Chat>> GetChatsByUserIdAsync(Guid userId);

        /// <summary>
        /// Добавляет чат с участниками
        /// </summary>
        Task AddChatWithParticipantsAsync(Chat chat, IEnumerable<Guid> participantUserIds);
    }
}
