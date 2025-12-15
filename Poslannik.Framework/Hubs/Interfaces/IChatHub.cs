using Poslannik.Framework.Models;


namespace Poslannik.Framework.Hubs.Interfaces;

/// <summary>
/// Интерфейс хаба для системы обмена сообщениями в реальном времени
/// </summary>
public interface IChatHub
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
    /// <param name="participantUserIds">Идентификаторы участников (для групповых чатов)</param>
    /// <returns>Созданный чат с идентификатором</returns>
    Task<Chat> CreateChatAsync(Chat chat, IEnumerable<Guid>? participantUserIds = null);

    Task UpdateChatAsync(Chat chat);

    Task DeleteChatAsync(Guid chatId);

    /// <summary>
    /// Уведомляет участников чата о событии
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="chat">Данные чата для отправки</param>
    Task NotifyChatParticipantsAsync(Chat chat);

    /// <summary>
    /// Получает список участников чата
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <returns>Список участников</returns>
    Task<IEnumerable<ChatParticipant>> GetChatParticipantsAsync(Guid chatId);

    /// <summary>
    /// Удаляет участника из чата
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task RemoveParticipantAsync(Guid chatId, Guid userId);

    /// <summary>
    /// Передает права администратора другому участнику
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="newAdminId">Идентификатор нового администратора</param>
    Task TransferAdminRightsAsync(Guid chatId, Guid newAdminId);

    /// <summary>
    /// Добавляет участников в существующий групповой чат
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="participantUserIds">Идентификаторы добавляемых участников</param>
    Task AddParticipantsAsync(Guid chatId, IEnumerable<Guid> participantUserIds);
}
