namespace Cube.Application.Services.Image.Dto
{
    public class UpdateImageDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] NewImage { get; set; }
    }
}
