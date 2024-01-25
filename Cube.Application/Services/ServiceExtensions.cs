using Cube.Application.Services.Chat;
using Cube.Application.Services.Message;
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
    }
}
