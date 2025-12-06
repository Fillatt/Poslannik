using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;

namespace Poslannik.DataBase;

public class ApplicationContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<MessageEntity> Messages => Set<MessageEntity>();
    public DbSet<ChatEntity> Chats => Set<ChatEntity>();
    public DbSet<ChatParticipantEntity> ChatParticipants => Set<ChatParticipantEntity>();

    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}
