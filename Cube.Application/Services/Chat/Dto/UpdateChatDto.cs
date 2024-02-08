namespace Cube.Application.Services.Chat.Dto
{
    // if field is null -> nothing to update
    public class UpdateChatDto
    {
        public int Id { get; set; }
        public bool IsModified { get; set; } 
        public string? NewTitle { get; set; }
        public ICollection<int>? RemovedParticipants { get; set; }
        public ICollection<int>? NewParticipants { get; set; }
    }
}
