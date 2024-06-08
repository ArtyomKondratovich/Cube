namespace Cube.Services.Services.Message.Dto
{
    public class UpdateMessageDto
    {
        public int MessageId { get; set; }
        public int UpdaterId { get; set; }
        public string NewMessage { get; set; }
        public string OldMessage { get; set; }
    }
}
