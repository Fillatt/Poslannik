using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repo
{
    namespace Poslannik.DataBase.Repositories
    {
        public class ChatParticipantRepo
        {
            private readonly ApplicationContext _dbContext;

            public ChatParticipantRepo(ApplicationContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<ChatParticipant?> GetChatParticipant(Guid chatId, Guid userId, CancellationToken cancellationToken)
            {
                return _dbContext.ChatParticipants
                    .Include(cp => cp.User) // Добавляем включение User
                    .Include(cp => cp.Chat) // Добавляем включение Chat
                    .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId, cancellationToken);
            }

            public Task<List<ChatParticipant>> GetChatParticipants(Guid chatId, CancellationToken cancellationToken)
            {
                return _dbContext.ChatParticipants
                    .Where(cp => cp.ChatId == chatId)
                    .Include(cp => cp.User) // Добавляем включение User
                    .ToListAsync(cancellationToken);
            }

            public Task<List<Guid>> GetChatParticipantIds(Guid chatId, CancellationToken cancellationToken)
            {
                return _dbContext.ChatParticipants
                    .Where(cp => cp.ChatId == chatId)
                    .Select(cp => cp.UserId)
                    .ToListAsync(cancellationToken);
            }

            public Task<List<User>> GetChatUsers(Guid chatId, CancellationToken cancellationToken)
            {
                return _dbContext.ChatParticipants
                    .Where(cp => cp.ChatId == chatId)
                    .Include(cp => cp.User)
                    .Select(cp => cp.User)
                    .ToListAsync(cancellationToken);
            }

            public async Task AddParticipantToChat(ChatParticipant participant, CancellationToken cancellationToken)
            {
                _dbContext.ChatParticipants.Add(participant);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

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

            public async Task RemoveAllParticipantsFromChat(Guid chatId, CancellationToken cancellationToken)
            {
                var participants = await _dbContext.ChatParticipants
                    .Where(cp => cp.ChatId == chatId)
                    .ToListAsync(cancellationToken);

                _dbContext.ChatParticipants.RemoveRange(participants);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            public Task<bool> IsUserInChat(Guid chatId, Guid userId, CancellationToken cancellationToken)
            {
                return _dbContext.ChatParticipants
                    .AnyAsync(cp => cp.ChatId == chatId && cp.UserId == userId, cancellationToken);
            }
        }
    }
}
