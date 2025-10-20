using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IChatParticipantRepo
    {
        /// <summary>
        /// Получает участника чата по идентификаторам чата и пользователя
        /// </summary>
        Task<ChatParticipant?> GetChatParticipant(Guid chatId, Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает всех участников чата
        /// </summary>
        Task<List<ChatParticipant>> GetChatParticipants(Guid chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает идентификаторы всех участников чата
        /// </summary>
        Task<List<Guid>> GetChatParticipantIds(Guid chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает всех пользователей-участников чата
        /// </summary>
        Task<List<User>> GetChatUsers(Guid chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет участника в чат
        /// </summary>
        Task AddParticipantToChat(ChatParticipant participant, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет участника из чата
        /// </summary>
        Task RemoveParticipantFromChat(Guid chatId, Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет всех участников из чата
        /// </summary>
        Task RemoveAllParticipantsFromChat(Guid chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет, является ли пользователь участником чата
        /// </summary>
        Task<bool> IsUserInChat(Guid chatId, Guid userId, CancellationToken cancellationToken);
    }
}