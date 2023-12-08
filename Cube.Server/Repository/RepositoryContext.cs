using Cube.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Cube.Server.Repository
{
    public class RepositoryContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<MessageModel> Messages { get; set; }

        public RepositoryContext(DbContextOptions options) :
            base(options)
        {
            // Database.EnsureCreated();
        }
    }
}
