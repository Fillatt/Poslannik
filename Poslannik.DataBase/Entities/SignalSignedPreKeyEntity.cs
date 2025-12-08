using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poslannik.DataBase.Entities
{
    /// <summary>
    /// Entity для хранения подписанных предварительных ключей Signal Protocol
    /// </summary>
    public class SignalSignedPreKeyEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public int SignedPreKeyId { get; set; }

        [Required]
        public required byte[] PublicKey { get; set; }

        [Required]
        public required byte[] PrivateKey { get; set; }

        [Required]
        public required byte[] Signature { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        // Навигационное свойство
        public virtual UserEntity User { get; set; } = null!;
    }
}
