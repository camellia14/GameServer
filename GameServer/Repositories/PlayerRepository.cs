using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using GameServer.DB;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _dbContext;
        public PlayerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlayerEntity?> GetByIdAsync(int userId)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.UserId == userId);
        }
        public async Task Create(PlayerEntity player)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(PlayerEntity player)
        {
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
        }
    }
}