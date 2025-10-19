using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;
using Poslannik.DataBase.Repositories.Interfaces;

namespace Poslannik.DataBase.Repo
{
    public class MessageRepo : IMessageRepo
    {
        private readonly ApplicationContext _dbContext;

        public MessageRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получает сообщение по идентификатору
        /// </summary>
        public Task<Message?> GetMessageById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Messages
                .Include(m => m.Sender)
                .Include(m => m.Chat)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        /// <summary>
        /// Получает все сообщения чата
        /// </summary>
        public Task<List<Message>> GetChatMessages(Guid chatId, CancellationToken cancellationToken)
        {
            return _dbContext.Messages
                .Where(m => m.ChatId == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.Id)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает сообщения чата с пагинацией
        /// </summary>
        public Task<List<Message>> GetChatMessagesPaged(Guid chatId, int skip, int take, CancellationToken cancellationToken)
        {
            return _dbContext.Messages
                .Where(m => m.ChatId == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает все сообщения пользователя
        /// </summary>
        public Task<List<Message>> GetUserMessages(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.Messages
                .Where(m => m.SenderId == userId)
                .Include(m => m.Chat)
                .Include(m => m.Sender)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Создает новое сообщение
        /// </summary>
        public async Task<Message> CreateMessage(Message message, CancellationToken cancellationToken)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return message;
        }

        /// <summary>
        /// Удаляет сообщение по идентификатору
        /// </summary>
        public async Task DeleteMessage(Guid messageId, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);

            if (message != null)
            {
                _dbContext.Messages.Remove(message);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Удаляет все сообщения чата
        /// </summary>
        public async Task DeleteChatMessages(Guid chatId, CancellationToken cancellationToken)
        {
            var messages = await _dbContext.Messages
                .Where(m => m.ChatId == chatId)
                .ToListAsync(cancellationToken);

            _dbContext.Messages.RemoveRange(messages);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}