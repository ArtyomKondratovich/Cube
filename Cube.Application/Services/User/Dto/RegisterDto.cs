using System.ComponentModel.DataAnnotations;

namespace Cube.Application.Services.User.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
