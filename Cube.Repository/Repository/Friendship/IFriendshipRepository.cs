using Cube.Core.Entities;

namespace Cube.Repository.Repository.Friendship
{
    public interface IFriendshipRepository
    {
        public Task<FriendshipEntity> GetFriendshipByIdAsync(int id);
        public Task<List<FriendshipEntity>> GetUsersFriendships(int userId); 
        public Task<FriendshipEntity> CreateFriendshipAsync(FriendshipEntity friendship);
        public Task<bool> DeleteFriendshipAsync(int id);
    }
}
