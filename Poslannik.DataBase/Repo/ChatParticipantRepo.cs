using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;

namespace Poslannik.DataBase.Repo
{
    public class ChatParticipantRepo : IChatParticipantRepo
    {
        private readonly ApplicationContext _dbContext;

        public ChatParticipantRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получает участника чата по идентификаторам чата и пользователя
        /// </summary>
        public Task<ChatParticipant?> GetChatParticipant(Guid chatId, Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .Include(cp => cp.User)
                .Include(cp => cp.Chat)
                .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId, cancellationToken);
        }

        /// <summary>
        /// Получает всех участников чата
        /// </summary>
        public Task<List<ChatParticipant>> GetChatParticipants(Guid chatId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .Where(cp => cp.ChatId == chatId)
                .Include(cp => cp.User)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает идентификаторы всех участников чата
        /// </summary>
        public Task<List<Guid>> GetChatParticipantIds(Guid chatId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .Where(cp => cp.ChatId == chatId)
                .Select(cp => cp.UserId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получает всех пользователей-участников чата
        /// </summary>
        public Task<List<User>> GetChatUsers(Guid chatId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .Where(cp => cp.ChatId == chatId)
                .Include(cp => cp.User)
                .Select(cp => cp.User)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавляет участника в чат
        /// </summary>
        public async Task AddParticipantToChat(ChatParticipant participant, CancellationToken cancellationToken)
        {
            _dbContext.ChatParticipants.Add(participant);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет участника из чата
        /// </summary>
        public async Task RemoveParticipantFromChat(Guid chatId, Guid userId, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.ChatParticipants
                .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId, cancellationToken);

            if (participant != null)
            {
                _dbContext.ChatParticipants.Remove(participant);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Удаляет всех участников из чата
        /// </summary>
        public async Task RemoveAllParticipantsFromChat(Guid chatId, CancellationToken cancellationToken)
        {
            var participants = await _dbContext.ChatParticipants
                .Where(cp => cp.ChatId == chatId)
                .ToListAsync(cancellationToken);

            _dbContext.ChatParticipants.RemoveRange(participants);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Проверяет, является ли пользователь участником чата
        /// </summary>
        public Task<bool> IsUserInChat(Guid chatId, Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .AnyAsync(cp => cp.ChatId == chatId && cp.UserId == userId, cancellationToken);
        }
    }
}