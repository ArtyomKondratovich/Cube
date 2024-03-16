namespace Cube.Application.Services.Image.Dto
{
    public class NewImageDto
    {
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public byte[] Image { get; set; }
    }
}
