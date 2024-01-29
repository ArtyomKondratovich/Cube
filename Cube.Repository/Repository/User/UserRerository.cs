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

        public async Task<UserModel?> GetUserById(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
    }
}
