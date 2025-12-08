using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poslannik.DataBase.Entities
{
    /// <summary>
    /// Entity для хранения идентификационных ключей Signal Protocol
    /// </summary>
    public class SignalIdentityKeyEntity
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public required byte[] PublicKey { get; set; }

        [Required]
        public required byte[] PrivateKey { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        // Навигационное свойство
        public virtual UserEntity User { get; set; } = null!;
    }
}
