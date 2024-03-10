using Microsoft.EntityFrameworkCore;
using Cube.Core.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Cube.Core.Utilities;
using Cube.Core.Entities;
using System.Reflection.Emit;

namespace Cube.EntityFramework
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<RoleEntity> Roles { get; set; } 

        public CubeDbContext(DbContextOptions options) :
            base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
