using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Entities
{
    public class ChatEntity
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

        public Guid? AdminId { get; set; }

        // Навигационные свойства
        public virtual ICollection<ChatParticipantEntity> Participants { get; set; } = new List<ChatParticipantEntity>();
        public virtual ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}