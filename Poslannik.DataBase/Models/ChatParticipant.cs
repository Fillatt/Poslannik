using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Models
{
    public class ChatParticipant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Chat")]
        public Guid ChatId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public byte[]? UserEncryptedKey { get; set; }

        // Навигационные свойства
        public virtual Chat Chat { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
