using System.ComponentModel.DataAnnotations;

namespace Cube.Core.Entities
{
    public class ImageEntity
    {
        [Key]
        public string Name { get; set; }

        public byte[] ImageBytes { get; set; } 
    }
}
