using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Poslannik.DataBase.Entities;

public class ChatParticipantEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("Chat")]
    public Guid ChatId { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    // Навигационные свойства
    public virtual ChatEntity Chat { get; set; } = null!;
    public virtual UserEntity User { get; set; } = null!;
}