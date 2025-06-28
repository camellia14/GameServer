using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;

namespace GameServer.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public Task<ItemEntity?> GetByIdAsync(int userId)
        {
            // 実装はデータベースやストレージからアイテムを取得するロジックを記述
            return Task.FromResult<ItemEntity?>(null);
        }
        public Task UpdateAsync(ItemEntity item)
        {
            // 実装はデータベースやストレージにアイテムの変更を保存するロジックを記述
            return Task.CompletedTask;
        }
    }
} 