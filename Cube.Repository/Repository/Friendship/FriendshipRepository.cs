using Cube.Core.Entities;
using Cube.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cube.Repository.Repository.Friendship
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly CubeDbContext _context;
        
        public FriendshipRepository(CubeDbContext context) 
        {
            _context = context;
        }

        public async Task<FriendshipEntity> CreateFriendshipAsync(FriendshipEntity friendship)
        {
            var result = await _context.Friendships.AddAsync(friendship);

            if (result != null) 
            {
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteFriendshipAsync(int id)
        {
            
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(x => x.Id == id);

            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }

            return friendship != null;
        }

        public async Task<FriendshipEntity> GetFriendshipByIdAsync(int id)
        {
            return await _context.Friendships.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FriendshipEntity>> GetUsersFriendshipsAsync(int userId)
        {
            return await _context.Friendships
                .Where(x => x.FirstUserId == userId || x.SecondUserId == userId)
                .ToListAsync();
        }
    }
}
