using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.User
{
    public class UserPerository : IUserRepository
    {
        private readonly CubeDbContext _dbContext;

        public UserPerository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            return user;
        }
    }
}
