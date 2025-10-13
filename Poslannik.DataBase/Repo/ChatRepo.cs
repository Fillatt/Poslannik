using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repo
{
    public class ChatRepo
    {
        private readonly ApplicationContext _dbContext;

        public ChatRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Chat?> GetChatById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Chats
                .Include(c => c.Participants) // Добавляем включение Participants
                    .ThenInclude(p => p.User) // И их User
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task<List<Chat>> GetUserChats(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.ChatParticipants
                .Where(cp => cp.UserId == userId)
                .Include(cp => cp.Chat)
                    .ThenInclude(c => c.Participants)
                        .ThenInclude(p => p.User)
                .Select(cp => cp.Chat)
                .ToListAsync(cancellationToken);
        }

        public Task<Chat?> GetPrivateChatBetweenUsers(Guid user1Id, Guid user2Id, CancellationToken cancellationToken)
        {
            return _dbContext.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c =>
                    c.ChatType == 1 &&
                    ((c.User1Id == user1Id && c.User2Id == user2Id) ||
                     (c.User1Id == user2Id && c.User2Id == user1Id)),
                    cancellationToken);
        }

        public async Task<Chat> CreateChat(Chat chat, CancellationToken cancellationToken)
        {
            _dbContext.Chats.Add(chat);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return chat;
        }

        public async Task DeleteChat(Guid chatId, CancellationToken cancellationToken)
        {
            var chat = await _dbContext.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId, cancellationToken);

            if (chat != null)
            {
                _dbContext.Chats.Remove(chat);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
