using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Entities
{
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

        [Required]
        public required byte[] EncryptedMessage { get; set; }

        // Навигационные свойства
        public virtual ChatEntity Chat { get; set; } = null!;

        [ForeignKey("SenderId")]
        public virtual UserEntity Sender { get; set; } = null!;
    }
}