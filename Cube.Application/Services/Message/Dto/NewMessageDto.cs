namespace Cube.Application.Services.Message.Dto
{
    public class NewMessageDto
    {
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string Message { get; set; }
        public int TimeZoneOffset { get; set; }
    }
}
