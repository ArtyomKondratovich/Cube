using Cube.Server.Models;
using Cube.Server.Models.Dto;
using Cube.Server.Models.ResultObjects;
using Cube.Server.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Server.Repository.Implementations
{
    public class MessageRepository : IMessageRepository
    {
        private readonly RepositoryContext _contex;

        public MessageRepository(RepositoryContext contex) 
        {
            _contex = contex;
        }

        public Result DeleteMessage(DeleteMessageDto dto)
        {
            var result = new Result()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            var message = _contex.Messages.Find(dto.Id);

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

        public Result SendMessage(NewMessageDto dto)
        {
            var result = new Result()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            if (_contex.Users != null)
            {
                var sender = _contex.Users.Find(dto.SenderId);
                var reciever = _contex.Users.Find(dto.RecieverId);

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

        public Result UpdateMessage(UpdateMessageDto dto)
        {
            var result = new Result()
            {
                ReturnObject = false,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            var message = _contex.Messages.Find(dto.Id);

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
