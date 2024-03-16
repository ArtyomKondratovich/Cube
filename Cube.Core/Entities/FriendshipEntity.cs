using System.ComponentModel.DataAnnotations;

namespace Cube.Core.Entities
{
    public class FriendshipEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public UserEntity User { get; set; }
        public UserEntity Friend { get; set; }
    }
}
