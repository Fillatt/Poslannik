using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Poslannik.DataBase.Entities;

public class MessageEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("Chat")]
    public Guid ChatId { get; set; }

    [Required]
    [ForeignKey("Sender")]
    public Guid SenderId { get; set; }

    public string Data { get; set; }

    public DateTime DateTime { get; set; }

    // Навигационные свойства
    public virtual ChatEntity Chat { get; set; } = null!;

    [ForeignKey("SenderId")]
    public virtual UserEntity Sender { get; set; } = null!;
}