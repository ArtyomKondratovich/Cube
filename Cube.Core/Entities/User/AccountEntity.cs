using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cube.Core.Models.User
{
    public class AccountEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }
}
