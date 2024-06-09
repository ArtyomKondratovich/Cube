using Cube.Domain.Enums;

namespace Cube.Business.Services.Image.Dto
{
    public class FindImageDto
    {
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
