using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Models
{
    public class Chat
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int ChatType { get; set; } // 1 = приватный, 2 = групповой

        // Поля для приватных чатов
        public Guid? User1Id { get; set; }
        public Guid? User2Id { get; set; }

        // Поля для групповых чатов
        [MaxLength(200)]
        public string? Name { get; set; }

        public byte[]? EncryptedGroupKey { get; set; }

        public Guid? AdminId { get; set; }

        // Навигационные свойства
        public virtual ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
