using Cube.Application.Services.Chat;
using Cube.Application.Services.Message;
using Cube.Application.Services.User;
using Cube.Core.Models;
using Cube.Core.Models.User;
using Cube.EntityFramework.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Cube.Application.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMessageService>(
                options => new MessageService(
                    options.GetRequiredService<IRepositoryWrapper>()
                    ));
            builder.Services.AddScoped<IChatService>(
                options => new ChatService(
                    options.GetRequiredService<IRepositoryWrapper>()
                    ));
            builder.Services.AddScoped<IUserService>(
                options => new UserService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    options.GetRequiredService<IOptions<AuthOptions>>()
                    ));
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

        public static void ConfigureAuth(this WebApplicationBuilder builder)
        {
            var authConfig = builder.Configuration.GetSection("Auth");
            builder.Services.Configure<AuthOptions>(authConfig);
            var authOptions = authConfig.Get<AuthOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options => 
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = authOptions.SummetricKey,
                        ValidateIssuerSigningKey = true
                    };
                });
            builder.Services.AddCors(options => 
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public static string GetHash(this string plainText)
        {
            var sha = new SHA1Managed();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return Convert.ToBase64String(hash);
        }
    }
}
