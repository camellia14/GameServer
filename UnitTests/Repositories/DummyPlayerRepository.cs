using GameServer.Entities;
using GameServer.Repositories.Interfaces;

namespace UnitTests.Repositories
{
    public class DummyPlayerRepository : IPlayerRepository
    {
        public async Task<PlayerEntity?> GetPlayerAsync(int userId)
        {
            return await Task.FromResult<PlayerEntity?>(null);
        }

        public async Task<PlayerEntity?> GetByIdAsync(int userId)
        {
            return await Task.FromResult<PlayerEntity?>(null);
        }

        public async Task<PlayerEntity> CreatePlayerAsync(PlayerEntity player)
        {
            return await Task.FromResult(player);
        }

        public async Task Create(PlayerEntity player)
        {
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(PlayerEntity item)
        {
            await Task.CompletedTask;
        }
    }
} 