namespace Cube.Core.Models.User
{
    public class UserEntity
    {
        public int Id { get; set; }
        public AccountEntity Account { get; set; } // for authorization
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
