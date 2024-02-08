using Microsoft.EntityFrameworkCore;
using Cube.Core.Models;
using Cube.Core.Models.User;

namespace Cube.EntityFramework
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<AccountEntity> Accounts { get; set; } 

        public CubeDbContext(DbContextOptions<CubeDbContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
