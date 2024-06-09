using Microsoft.AspNetCore.Http;

namespace Cube.Business.Services.User.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IFormFile? File { get; set; }
    }
}
