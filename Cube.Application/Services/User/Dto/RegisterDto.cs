using System.ComponentModel.DataAnnotations;

namespace Cube.Application.Services.User.Dto
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
