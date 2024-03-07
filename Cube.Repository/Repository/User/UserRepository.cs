using Cube.Core.Models.User;
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

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public UserEntity? GetUserById(int id) 
        {
            return _dbContext.Users
                .FirstOrDefault(x => x.Id == id);
        }

        public UserEntity? UserAssociatedWithTheAccount(int accountId)
        {
            return _dbContext.Users
                .Where(x => x.Account.Id == accountId)
                .FirstOrDefault();
        }

        public async Task<UserEntity?> CreteUser(UserEntity user)
        {
            var result = await _dbContext.Users.AddAsync(user);

            if (result != null)
            {
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }
    }
}
