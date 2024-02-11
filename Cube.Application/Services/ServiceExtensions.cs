using AutoMapper.Configuration.Conventions;
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

        public static bool IsValidEmail(this string? email)
        {
            if (email == null)
            {
                return false;
            }

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static int PasswordCheck(this string? password) 
        {
            // 0 - for null passwrod
            // 1 - easy password
            // 2 - medium password
            // 3 - strong password

            if (password == null)
            {
                return 0;
            }

            return 1;
        }
    }
}
