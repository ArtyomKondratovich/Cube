using Cube.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cube.Domain.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
