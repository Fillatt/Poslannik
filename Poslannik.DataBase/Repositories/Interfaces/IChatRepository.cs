using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface IChatRepository : IRepository<ChatEntity, Chat>
{
    /// <summary>
    /// Получает все чаты пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Список чатов пользователя</returns>
    Task<IEnumerable<Chat>> GetChatsByUserIdAsync(Guid userId);

    /// <summary>
    /// Добавляет чат с участниками
    /// </summary>
    /// <param name="chat">Модель чата</param>
    /// <param name="participantUserIds">Идентификаторы участников</param>
    Task AddChatWithParticipantsAsync(Chat chat, IEnumerable<Guid> participantUserIds);

    /// <summary>
    /// Получает чат по идентификатору
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <returns>Чат или null, если не найден</returns>
    Task<Chat?> GetChatByIdAsync(Guid chatId);
}
