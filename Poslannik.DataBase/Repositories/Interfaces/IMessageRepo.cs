using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repositories.Interfaces
{
    public interface IMessageRepo
    {
        /// <summary>
        /// Получает сообщение по идентификатору
        /// </summary>
        Task<Message?> GetMessageById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает все сообщения чата
        /// </summary>
        Task<List<Message>> GetChatMessages(Guid chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сообщения чата с пагинацией
        /// </summary>
        Task<List<Message>> GetChatMessagesPaged(Guid chatId, int skip, int take, CancellationToken cancellationToken);

        /// <summary>
        /// Получает все сообщения пользователя
        /// </summary>
        Task<List<Message>> GetUserMessages(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Создает новое сообщение
        /// </summary>
        Task<Message> CreateMessage(Message message, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет сообщение по идентификатору
        /// </summary>
        Task DeleteMessage(Guid messageId, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет все сообщения чата
        /// </summary>
        Task DeleteChatMessages(Guid chatId, CancellationToken cancellationToken);
    }
}