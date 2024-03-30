using Cube.Core.Enums;
using System.Text.Json.Serialization;

namespace Cube.Core.Models.Image
{
    public class ImageModel
    {
        public int Id { get; set; }
        public byte[] ImgBytes { get; set; }
        public int OwnerId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ImageType Type { get; set; }
    }
}
