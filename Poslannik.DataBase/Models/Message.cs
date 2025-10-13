using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Models
{
    public class Message
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
        public virtual Chat Chat { get; set; } = null!;

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } = null!;
    }
}
