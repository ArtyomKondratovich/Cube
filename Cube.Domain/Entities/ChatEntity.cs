using Cube.Domain.Entities;
using Cube.Domain.Enums;

namespace Cube.Domain.Models
{
    public class ChatEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}