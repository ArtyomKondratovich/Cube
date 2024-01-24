using Microsoft.EntityFrameworkCore;
using Cube.Core.Models;

namespace Cube.EntityFramework
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<MessageModel> Messages { get; set; }

        public DbSet<ChatModel> Chats { get; set; }

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
