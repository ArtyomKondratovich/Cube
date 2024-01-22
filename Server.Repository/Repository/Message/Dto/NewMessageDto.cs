namespace Cube.EntityFramework.Repository.Message.Dto
{
    public class NewMessageDto
    {
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string Message { get; set; }
    }
}
