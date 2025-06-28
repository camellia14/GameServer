using System.Threading.Tasks;
using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<ItemEntity?> GetByIdAsync(int userId);
        Task UpdateAsync(ItemEntity item);
    }
} 