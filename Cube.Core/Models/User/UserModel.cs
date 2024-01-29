namespace Cube.Core.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public AccountModel Account { get; set; } // for authorization
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
