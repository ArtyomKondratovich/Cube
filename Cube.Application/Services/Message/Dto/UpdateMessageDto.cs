﻿namespace Cube.Application.Services.Message.Dto
{
    public class UpdateMessageDto
    {
        public int Id { get; set; }
        public int UpdaterId { get; set; }
        public string NewMessage { get; set; }
        public string OldMessage { get; set; }
    }
}
