using Microsoft.EntityFrameworkCore;
using GameServer.Entities;

namespace GameServer.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<CharacterEntity> Characters { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<StackItemEntity> StackItems { get; set; }
        public DbSet<UniqueItemEntity> UniqueItems { get; set; }
    }
}
