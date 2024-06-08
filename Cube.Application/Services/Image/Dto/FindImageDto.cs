using Cube.Core.Enums;

namespace Cube.Services.Services.Image.Dto
{
    public class FindImageDto
    {
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
