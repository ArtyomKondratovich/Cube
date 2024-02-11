namespace Cube.Core.Models.User
{
    public class UserAuthModel
    {
        public string token { get; set; }
        public UserEntity User { get; set; }
    }
}
