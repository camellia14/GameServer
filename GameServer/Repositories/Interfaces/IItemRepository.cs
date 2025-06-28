using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<ItemEntity?> GetByIdAsync(int itemId);
        Task<List<ItemEntity>> GetAllAsync();
        Task<ItemEntity> CreateAsync(ItemEntity item);
        Task UpdateAsync(ItemEntity item);
        Task DeleteAsync(int itemId);
        Task<bool> ExistsAsync(int itemId);
        Task<List<ItemEntity>> GetByNameAsync(string name);
    }
} 