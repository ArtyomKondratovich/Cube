using Cube.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Cube.DataAccess;
using Microsoft.EntityFrameworkCore;
using Cube.Business.Services.Chat;
using Cube.Business.Utilities;
using Cube.Business.Services.Message;
using Cube.Business.Services.User;
using Cube.Business.Services.User.Auth;
using Cube.Business.Services.Image;
using Cube.Business.Services.Notification;
using Cube.Business.Services.Email;

namespace Cube.Api.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            builder.Services.AddScoped<IMessageService>(
                options => new MessageService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    int.Parse(builder.Configuration.GetSection("MaxMessagesReceive").Value),
                    mapper
                )
            );

            builder.Services.AddScoped<IChatService>(
                options => new ChatService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    mapper
                )
            );

            builder.Services.AddScoped<IUserService>(
                options => new UserService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    options.GetRequiredService<IOptions<AuthOptions>>(),
                    mapper
                )
            );

            builder.Services.AddScoped<IImageService>(
                options => new ImageService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    builder.Configuration.GetSection("ImagesDirectoryPath").Value,
                    mapper
                )
            );

            builder.Services.AddScoped<INotificationService>(
                options => new NotificationService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    mapper
                )
            );


            #region Email Service

            var smtpSettings = builder.Configuration.GetSection("Smtp").Get<SmtpSettings>();

            builder.Services.AddScoped<IEmailService>(
                options => new EmailService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    smtpSettings
                )
            );

            #endregion
        }

        public static void ConfigureRepository(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CubeDbContext>(
                options => options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddScoped<IRepositoryWrapper>(
                options => new RepositoryWrapper(
                    options.GetRequiredService<CubeDbContext>()
                )
            );
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


