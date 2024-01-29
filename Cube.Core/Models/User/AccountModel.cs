using System.ComponentModel.DataAnnotations.Schema;

namespace Cube.Core.Models.User
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [NotMapped]
        public Role[] Roles { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }
}
