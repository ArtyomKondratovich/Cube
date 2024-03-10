using Cube.Core.Entities;

namespace Cube.Core.Models.User
{
    public class UserAuthModel
    {
        public string Token { get; set; }
        public UserEntity User { get; set; }
    }
}
