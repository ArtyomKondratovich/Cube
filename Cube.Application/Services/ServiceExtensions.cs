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
            builder.Services.AddScoped<IAuthService>(
                options => new AuthService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    options.GetRequiredService<IOptions<AuthOptions>>()
                    ));
        }

        public static bool IsEntitiesExist<TEntity>(this ICollection<int>? ids, IRepositoryWrapper wrapper)
            where TEntity : class
        {
            if (ids == null)
            {
                return true;
            }

            switch (typeof(TEntity))
            {
                case var t when t == typeof(MessageEntity):
                    
                    foreach (var id in ids)
                    {
                        if (wrapper.MessageRepository.GetMessageById(id) == null)
                        {
                            return false;
                        }
                    }
                    return true;
                case var t when t == typeof(ChatEntity):

                    foreach (var id in ids)
                    {
                        if (wrapper.ChatRepository.GetChatByIdAsync(id) == null)
                        {
                            return false;
                        }
                    }
                    return true;
                case var t when t == typeof(UserEntity):

                    foreach (var id in ids)
                    {
                        if (wrapper.UserRepository.GetUserByIdAsync(id) == null)
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

        public static bool TrySubParticipants(this ICollection<int> a, ICollection<int>? b, out ICollection<int>? result)
        {
            if (b == null)
            {
                result = a;
                return true;
            }

            if (a.Count < b.Count)
            {
                result = null;
                return false;
            }

            foreach (var number in b)
            {
                if (!a.Remove(number))
                {
                    result = null;
                    return false;
                }
            }

            result = a;
            return true;
        }

        public static ICollection<int> AddParticipants(this ICollection<int> a, ICollection<int>? b)
        {
            if (b == null)
            {
                return a;
            }

            foreach (var number in b)
            {
                if (a.Contains(number))
                {
                    continue;
                }

                a.Add(number);
            }

            return a;
        }
    }
}
