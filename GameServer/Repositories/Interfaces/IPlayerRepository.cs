using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity?> GetPlayerAsync(int userId);
        Task<PlayerEntity?> GetByIdAsync(int userId);
        Task<PlayerEntity> CreatePlayerAsync(PlayerEntity player);
        Task Create(PlayerEntity player);
        Task UpdateAsync(PlayerEntity player);
    }
} 