using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poslannik.DataBase.Entities
{
    /// <summary>
    /// Entity для хранения сессий шифрования Signal Protocol
    /// </summary>
    public class SignalSessionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey("Recipient")]
        public Guid RecipientId { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [Required]
        public required byte[] SessionState { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        // Навигационные свойства
        public virtual UserEntity User { get; set; } = null!;

        [ForeignKey("RecipientId")]
        public virtual UserEntity Recipient { get; set; } = null!;
    }
}
