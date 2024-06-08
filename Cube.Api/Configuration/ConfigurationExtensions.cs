﻿using Cube.Services.Services.Chat;
using Cube.Services.Services.Message;
using Cube.Services.Services.User;
using Cube.Repository.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cube.Services.Utilities;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Cube.Repository;
using Microsoft.EntityFrameworkCore;
using Cube.Services.Services.User.Auth;
using Cube.Services.Services.Image;
using Cube.Services.Services.Notification;
using Cube.Services.Services.Email;

namespace Cube.Web.Api.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMessageService>(
                options => new MessageService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    int.Parse(builder.Configuration.GetSection("MaxMessagesReceive").Value)
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
            builder.Services.AddScoped<IImageService>(
                options => new ImageService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    builder.Configuration.GetSection("ImagesDirectoryPath").Value
                    ));

            builder.Services.AddScoped<INotificationService>(
                options => new NotificationService(
                    options.GetRequiredService<IRepositoryWrapper>()));

            builder.Services.AddScoped<IEmailService>(
                options => new EmailConsoleService(
                    options.GetRequiredService<IRepositoryWrapper>(),
                    builder.Configuration));
        }

        public static void ConfigureRepository(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CubeDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepositoryWrapper>(
                options => new RepositoryWrapper(options.GetRequiredService<CubeDbContext>()));
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


