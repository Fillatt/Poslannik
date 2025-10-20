using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repo
{
    public interface IChatRepo
    {
        /// <summary>
        /// Получает чат по идентификатору
        /// </summary>
        Task<Chat?> GetChatById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает все чаты пользователя
        /// </summary>
        Task<List<Chat>> GetUserChats(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает приватный чат между двумя пользователями
        /// </summary>
        Task<Chat?> GetPrivateChatBetweenUsers(Guid user1Id, Guid user2Id, CancellationToken cancellationToken);

        /// <summary>
        /// Создает новый чат
        /// </summary>
        Task<Chat> CreateChat(Chat chat, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет чат по идентификатору
        /// </summary>
        Task DeleteChat(Guid chatId, CancellationToken cancellationToken);
    }
}