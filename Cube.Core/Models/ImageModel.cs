using Cube.Core.Enums;

namespace Cube.Core.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int OwnerId { get; set; }
        public ImageType Type { get; set; }
    }
}
