﻿using Cube.Core.Entities;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;
using System.Security.Cryptography;
using System.Text;

namespace Cube.Application.Services
{
    public static class ServiceExtensions
    {
        public static async Task<bool> IsEntitiesExist<TEntity>(this ICollection<int>? ids, IRepositoryWrapper wrapper)
            where TEntity : class
        {
            if (ids == null)
            {
                return false;
            }

            var repositoryMethodMap = new Dictionary<Type, Func<int, Task<object?>>>
            {
                { typeof(MessageEntity), async id => await wrapper.MessageRepository.GetMessageById(id) },
                { typeof(ChatEntity), async id => await wrapper.ChatRepository.GetChatByIdAsync(id) },
                { typeof(UserEntity), async id => await wrapper.UserRepository.GetUserByIdAsync(id) }
            };

            if (!repositoryMethodMap.TryGetValue(typeof(TEntity), out var repositoryMethod))
            {
                return false; // Unsupported entity type
            }

            foreach (var id in ids)
            {
                if (await repositoryMethod(id) == null)
                {
                    return false;
                }
            }

            return true;
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

        public static async Task<bool> IsEntityExist(this int id, string entityName, IRepositoryWrapper wrapper)
        {
            return false;
        }
    }
}
