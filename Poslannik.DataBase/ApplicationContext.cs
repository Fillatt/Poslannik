using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;

namespace Poslannik.DataBase;

public class ApplicationContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<MessageEntity> Messages => Set<MessageEntity>();
    public DbSet<ChatEntity> Chats => Set<ChatEntity>();
    public DbSet<ChatParticipantEntity> ChatParticipants => Set<ChatParticipantEntity>();
    public DbSet<SignalIdentityKeyEntity> SignalIdentityKeys => Set<SignalIdentityKeyEntity>();
    public DbSet<SignalPreKeyEntity> SignalPreKeys => Set<SignalPreKeyEntity>();
    public DbSet<SignalSignedPreKeyEntity> SignalSignedPreKeys => Set<SignalSignedPreKeyEntity>();
    public DbSet<SignalSessionEntity> SignalSessions => Set<SignalSessionEntity>();

    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка индексов для Signal таблиц
        modelBuilder.Entity<SignalPreKeyEntity>()
            .HasIndex(e => new { e.UserId, e.PreKeyId })
            .IsUnique();

        modelBuilder.Entity<SignalPreKeyEntity>()
            .HasIndex(e => new { e.UserId, e.IsUsed });

        modelBuilder.Entity<SignalSignedPreKeyEntity>()
            .HasIndex(e => new { e.UserId, e.SignedPreKeyId })
            .IsUnique();

        modelBuilder.Entity<SignalSessionEntity>()
            .HasIndex(e => new { e.UserId, e.RecipientId, e.DeviceId })
            .IsUnique();

        modelBuilder.Entity<MessageEntity>()
            .HasIndex(e => new { e.ChatId, e.SentAt });
    }
}
