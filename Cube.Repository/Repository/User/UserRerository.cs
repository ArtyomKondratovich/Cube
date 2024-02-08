using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User
{
    public class UserRerository : IUserRepository
    {
        private readonly CubeDbContext _dbContext;

        public UserRerository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public UserEntity? GetUserById(int id) 
        {
            return _dbContext.Users.Find(id);
        }
    }
}
