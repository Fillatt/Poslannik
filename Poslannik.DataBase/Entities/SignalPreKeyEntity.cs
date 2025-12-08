using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poslannik.DataBase.Entities
{
    /// <summary>
    /// Entity для хранения одноразовых предварительных ключей Signal Protocol
    /// </summary>
    public class SignalPreKeyEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public int PreKeyId { get; set; }

        [Required]
        public required byte[] PublicKey { get; set; }

        [Required]
        public required byte[] PrivateKey { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        // Навигационное свойство
        public virtual UserEntity User { get; set; } = null!;
    }
}
