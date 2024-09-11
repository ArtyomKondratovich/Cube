using Microsoft.EntityFrameworkCore;
using Cube.Domain.Models;
using Cube.Domain.Entities;
using Cube.Domain.Enums;

namespace Cube.DataAccess
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<FriendshipEntity> Friendships { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<NotificationEntity> Notifications { get; set; }

        public DbSet<ConfigEntity> Configs { get; set; }

        public DbSet<EmailTokenEntity> EmailTokens { get; set; }

        public CubeDbContext(DbContextOptions options) :
            base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FriendshipEntity>()
            .HasOne(f => f.FirstUser)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.FirstUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendshipEntity>()
                .HasOne(f => f.SecondUser)
                .WithMany()
                .HasForeignKey(f => f.SecondUserId)
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

            modelBuilder.Entity<NotificationEntity>()
                .Property(e => e.Accepted)
                .HasColumnType("BOOLEAN");

            modelBuilder.Entity<NotificationEntity>()
                .Property(e => e.IsReaded)
                .HasColumnType("BOOLEAN");

            base.OnModelCreating(modelBuilder);
        }
    }
}
