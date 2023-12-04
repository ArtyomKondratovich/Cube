using Cube.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Cube.Server.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) :
            base(options)
        {
        }

        DbSet<UserModel>? Users { get; set; }
        DbSet<MessageModel>? Messages { get; set; }
    }
}
