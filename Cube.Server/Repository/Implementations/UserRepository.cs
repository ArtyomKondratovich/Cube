using Cube.Server.Models;
using Cube.Server.Repository.Interfaces;

namespace Cube.Server.Repository.Implementations
{
    public class UserRepository : RepositoryBase<UserModel>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : 
            base(context) 
        {
        }
    }
}
