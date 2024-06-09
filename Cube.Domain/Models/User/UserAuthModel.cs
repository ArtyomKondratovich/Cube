namespace Cube.Domain.Models.User
{
    public class UserAuthModel
    {
        public string Token { get; set; }
        public UserModel User { get; set; }
    }
}
