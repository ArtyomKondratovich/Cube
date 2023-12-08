namespace Cube.Server.Models.Dto
{
    public class UpdateMessageDto
    {
        public int Id { get; set; }
        public string NewMessage { get; set; }
        public string OldMessage { get; set; }
    }
}
