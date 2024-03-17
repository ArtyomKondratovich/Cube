using Cube.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Cube.Application.Services.Image.Dto
{
    public class NewImageDto
    {
        public IFormFile File { get; set; }
        public byte[] ImgBytes { get; set; }
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
