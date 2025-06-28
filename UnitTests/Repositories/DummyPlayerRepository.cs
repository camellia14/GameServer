using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;

namespace UnitTests.Repositories
{
    public class DummyPlayerRepository : IPlayerRepository
    {
        public async Task<PlayerEntity?> GetByIdAsync(int userId)
        {
            // 実装はデータベースやストレージからアイテムを取得するロジックを記述
            return await Task.FromResult<PlayerEntity?>(null);
        }
        public async Task Create(PlayerEntity player)
        {
            // 実装はデータベースやストレージにアイテムを保存するロジックを記述
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(PlayerEntity item)
        {
            // 実装はデータベースやストレージにアイテムの変更を保存するロジックを記述
            await Task.CompletedTask;
        }
    }
} 