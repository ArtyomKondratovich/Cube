using Microsoft.EntityFrameworkCore;
using Cube.Core.Models;
using Cube.Core.Entities;
using Cube.Core.Enums;

namespace Cube.EntityFramework
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<FriendshipEntity> Friendships { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public CubeDbContext(DbContextOptions options) :
            base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = 1, Name = "User" },
                new RoleEntity { Id = 2, Name = "Admin" });

            modelBuilder.Entity<FriendshipEntity>()
            .HasOne(f => f.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendshipEntity>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImageEntity>()
                .Property(i => i.Type)
                .HasConversion(
                    t => t.ToString(),
                    t => (ImageType)Enum.Parse(typeof(ImageType), t));

            modelBuilder.Entity<ChatEntity>()
                .Property(i => i.Type)
                .HasConversion(
                    t => t.ToString(),
                    t => (ChatType)Enum.Parse(typeof(ChatType), t));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

    }
}
