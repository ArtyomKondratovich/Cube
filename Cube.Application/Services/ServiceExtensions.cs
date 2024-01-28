using Cube.Application.Services.Chat;
using Cube.Application.Services.Message;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cube.Application.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMessageService>(
                options => new MessageService(options.GetRequiredService<IRepositoryWrapper>()));
            builder.Services.AddScoped<IChatService>(
                options => new ChatService(options.GetRequiredService<IRepositoryWrapper>()));
        }

        public static bool Exists<TModel, TWrapper>(this ICollection<TModel> values, TWrapper wrapper)
            where TModel : class
            where TWrapper : IRepositoryWrapper
        {
            switch (typeof(TModel))
            {
                case var t when t == typeof(MessageModel):
                    
                    foreach (var item in values)
                    {
                        var message = (MessageModel)(object)item;
                        if (wrapper.MessageRepository.GetMessageById(message.Id) == null)
                        {
                            return false;
                        }
                    }
                    return true;
                case var t when t == typeof(ChatModel):

                    foreach (var item in values)
                    {
                        var chat = (ChatModel)(object)item;
                        if (wrapper.ChatRepository.GetChatById(chat.Id) == null)
                        {
                            return false;
                        }
                    }
                    return true;
                case var t when t == typeof(UserModel):

                    foreach (var item in values)
                    {
                        var user = (UserModel)(object)item;
                        if (wrapper.UserRepository.GetUserById(user.Id) == null)
                        {
                            return false;
                        }
                    }
                    return true;
            }

            return false;
        }

    }
}
