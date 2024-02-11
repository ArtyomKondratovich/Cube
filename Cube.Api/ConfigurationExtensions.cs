using Cube.Application.Services.Chat;
using Cube.Application.Services.Message;
using Cube.Application.Services.User;
using Cube.Application.Services;
using Cube.EntityFramework.Repository;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cube.Application.Utilities;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Cube.Web.Api
{
    public static class ConfigurationExtensions
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

        public static void ConfigureAuth(this WebApplicationBuilder builder)
        {
            var authConfig = builder.Configuration.GetSection("Auth");
            builder.Services.Configure<AuthOptions>(authConfig);
            var authOptions = authConfig.Get<AuthOptions>();
            builder.Services.AddAuthentication(options => 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters 
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
            builder.Services.AddAuthorization(options => 
            {
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
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

        public static void ConfigureJsonConverter(this WebApplicationBuilder builder) 
        {
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }
    }
}

    
