using Cube.Api.SiganlRHubs.dto;
using Cube.Business.Services.Chat;
using Cube.Business.Services.Message;
using Cube.Business.Services.Message.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Cube.Api.SiganlRHubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatHub(IChatService chatService, IMessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        public async Task JoinGroup([FromBody] JoinGroupDto dto)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dto.GroupName);
        }

        public async Task LeaveGroup([FromBody] LeaveGroupDto dto)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, dto.GroupName);
        }

        public async Task SendMessageToGroup([FromBody] NewMessageDto dto)
        {
            var result = await _messageService.SendMessageAsync(dto);

            await Clients.Group(dto.GroupName).SendAsync("ReceiveMessage", result);
        }
    }
}
