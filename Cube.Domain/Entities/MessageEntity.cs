using Cube.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cube.Domain.Models
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int ChatId { get; set; }
        public ChatEntity Chat { get; set; }
    }
}