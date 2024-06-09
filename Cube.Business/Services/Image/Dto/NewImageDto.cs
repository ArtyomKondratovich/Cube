using Cube.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Cube.Business.Services.Image.Dto
{
    public class NewImageDto
    {
        public IFormFile File { get; set; }
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
