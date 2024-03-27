namespace Cube.Core.Models.Messages
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string FormatedCreatedDate { get; set; }
        public string FormattedUpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
