using System.Threading.Tasks;
using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity?> GetByIdAsync(int userId);
        Task Create(PlayerEntity player);
        Task UpdateAsync(PlayerEntity player);
    }
} 