using System.ComponentModel.DataAnnotations;

namespace Cube.Application.Services.User.Dto
{
    public class SignUpDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
