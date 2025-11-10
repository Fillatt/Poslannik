using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Poslannik.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<MessageEntity> Messages => Set<MessageEntity>();
        public DbSet<ChatEntity> Chats => Set<ChatEntity>();
        public DbSet<ChatParticipantEntity> ChatParticipants => Set<ChatParticipantEntity>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
