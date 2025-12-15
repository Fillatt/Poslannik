using System.ComponentModel.DataAnnotations;

namespace Poslannik.DataBase.Entities;

public class ChatEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public int ChatType { get; set; } // 1 = приватный, 2 = групповой

    public Guid? User1Id { get; set; }

    public Guid? User2Id { get; set; }

    public string? Name { get; set; }

    public Guid? AdminId { get; set; }

    public virtual ICollection<ChatParticipantEntity> Participants { get; set; } = new List<ChatParticipantEntity>();

    public virtual ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}