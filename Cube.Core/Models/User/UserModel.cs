using Cube.Core.Entities;
using System.Text.Json.Serialization;

namespace Cube.Core.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public byte[]? AvatarBytes { get; set; }
    }
}
