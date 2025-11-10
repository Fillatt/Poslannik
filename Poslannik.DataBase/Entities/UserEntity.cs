using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Login { get; set; }

        [MaxLength(200)]
        public string? UserName { get; set; }

        [MaxLength(50)]
        public string? GroupUser { get; set; }

        [Required]
        public required byte[] PasswordHash { get; set; }

        [Required]
        public required byte[] PasswordSalt { get; set; }

        public byte[]? PublicKey { get; set; }
    }
}