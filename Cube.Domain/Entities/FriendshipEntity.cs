using System.ComponentModel.DataAnnotations;

namespace Cube.Domain.Entities
{
    public class FriendshipEntity
    {
        public int Id { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public UserEntity FirstUser { get; set; }
        public UserEntity SecondUser { get; set; }
    }
}
