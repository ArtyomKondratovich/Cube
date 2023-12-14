using Cube.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Cube.Core.Models;
using Cube.Application.Repository.Message.Dto;


namespace Cube.Application.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CubeDbContext _contex;

        public MessageRepository(CubeDbContext contex)
        {
            _contex = contex;
        }

        public async Task<Response> DeleteMessage(DeleteMessageDto dto)
        {
            var result = new Response()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            var message = await _contex.Messages.FindAsync(dto.Id);

            if (message == null)
            {
                result.Errors.Add("Message not found");
            }
            else
            {
                result.ReturnObject = true;
                result.ActionResult = new OkResult();
                _contex.Remove(message);
            }

            return result;
        }

        public async Task<Response> SendMessage(NewMessageDto dto)
        {
            var result = new Response()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            if (_contex.Users != null)
            {
                var sender = await _contex.Users.FindAsync(dto.SenderId);
                var reciever = await _contex.Users.FindAsync(dto.RecieverId);

                if (sender == null)
                {
                    result.Errors.Add("Sender not found");
                }

                if (reciever == null)
                {
                    result.Errors.Add("Reciever not found");
                }

                if (string.IsNullOrWhiteSpace(dto.Message))
                {
                    result.Errors.Add("Message is null or whitespace");
                }

                if (!result.Errors.Any())
                {
                    result.ReturnObject = true;
                    result.ActionResult = new OkResult();

                    var message = new MessageModel
                    {
                        Sender = sender,
                        Receiver = reciever,
                        Message = dto.Message,
                        CreatedDate = DateTime.UtcNow
                    };

                    _contex.Add(message);
                }
            }
            else
            {
                result.Errors.Add("Users not found");
            }

            return result;
        }

        public async Task<Response> UpdateMessage(UpdateMessageDto dto)
        {
            var result = new Response()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            var message = await _contex.Messages.FindAsync(dto.Id);

            if (message == null)
            {
                result.Errors.Add("Message not found");
            }
            else
            {
                result.ActionResult = new OkResult();
                result.ReturnObject = true;

                if (string.IsNullOrEmpty(dto.NewMessage))
                {
                    _contex.Remove(message);
                }
                else if (dto.NewMessage != dto.OldMessage)
                {
                    message.Message = dto.NewMessage;
                    message.UpdateDate = DateTime.UtcNow;
                    _contex.Update(message);
                }
            }

            return result;
        }
    }
}
