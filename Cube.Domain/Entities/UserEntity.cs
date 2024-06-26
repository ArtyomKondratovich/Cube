﻿using Cube.Domain.Models;
using System.Text.Json.Serialization;

namespace Cube.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        [JsonIgnore]
        public ICollection<ChatEntity> Chats { get; set; }
        [JsonIgnore]
        public ICollection<FriendshipEntity> Friends { get; set; }
    }
}
