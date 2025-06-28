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
    }
}
