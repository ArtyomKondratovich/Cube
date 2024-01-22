namespace Cube.EntityFramework.Repository.Message.Dto
{
    public class UpdateMessageDto
    {
        public int Id { get; set; }
        public string NewMessage { get; set; }
        public string OldMessage { get; set; }
    }
}
