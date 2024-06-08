using Cube.Core.Models.User;

namespace Cube.Services.Services.User.Dto
{
    public class NewUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AccountId { get; set; }
        public DateOnly? DateOfBirth { get; set; }

    }
}
