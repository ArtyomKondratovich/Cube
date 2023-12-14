

using Cube.Application.Repository.Chat.Dto;
using Cube.Application.Repository.User.Dto;
using Cube.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cube.Application.Repository.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly CubeDbContext _dbContext;

        public ChatRepository(CubeDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<Response> CreateChat(NewChatDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteChat(DeleteChatDto dto)
        {
            var response = new Response()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            //TODO implement delete chat

            return response;
        }

        public async Task<Response> GetAllUsersChats(FindUserDto dto)
        {
            var response = new Response()
            {
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            var user = await _dbContext.Users.FindAsync(dto.Id);

            if (user == null)
            {
                response.Errors.Add("User not found");
            }
            else 
            {
                var userChats = _dbContext.Chats.Where(chat => chat.Participants.Contains(user)).AsNoTracking();

                /*var userChats = _dbContext.Chats.Where(chat => chat.Participants
                .Any(participant => participant.Id == dto.Id))
                    .AsNoTracking();
                */

                response.ReturnObject = userChats;
                response.ActionResult = new OkResult();
            }

            return response;
        }


    }
}
