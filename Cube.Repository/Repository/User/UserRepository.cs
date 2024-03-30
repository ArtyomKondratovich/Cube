using Cube.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cube.EntityFramework.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly CubeDbContext _dbContext;

        public UserRepository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public UserEntity GetUserById(int id) 
        {
            return _dbContext.Users
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<UserEntity> UserAssociatedWithTheEmail(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserEntity> CreteUserAsync(UserEntity user)
        {
            var result = await _dbContext.Users.AddAsync(user);

            if (result != null)
            {
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<List<UserEntity>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstAsync(x => x.Id == id);

            if (user != null) {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }

            return user != null;
        }
    }
}
