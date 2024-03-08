using Microsoft.EntityFrameworkCore;
using Cube.Core.Models;
using Cube.Core.Models.User;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Cube.Core.Utilities;

namespace Cube.EntityFramework
{
    public class CubeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<AccountEntity> Accounts { get; set; } 

        public CubeDbContext(DbContextOptions options) :
            base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder
                .Entity<AccountEntity>()
                .Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<Role>());

            modelBuilder
                .Entity<ChatEntity>()
                .Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<ChatType>());

            modelBuilder.Entity<ChatEntity>()
                .HasMany(c => c.Participants)
                .WithMany(u => u.Chats)
                .UsingEntity<Dictionary<string, object>>(
                    "ChatParticipant",
                    j => j
                        .HasOne<UserEntity>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<ChatEntity>()
                        .WithMany()
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("ChatId", "UserId");
                        j.ToTable("ChatParticipants");
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
