using Cube.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cube.Core.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public ICollection<ChatEntity> Chats { get; set; }

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
