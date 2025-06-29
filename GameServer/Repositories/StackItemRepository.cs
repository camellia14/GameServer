using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    /// <summary>
    /// スタックアイテムリポジトリの実装クラス
    /// スタックアイテムのデータアクセス操作を提供する
    /// </summary>
    public class StackItemRepository : IStackItemRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// StackItemRepositoryのコンストラクタ
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public StackItemRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 指定されたプレイヤーのスタックアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>スタックアイテムのリスト</returns>
        public async Task<List<StackItemEntity>> GetPlayerStackItemsAsync(int playerUserId)
        {
            return await _context.StackItems
                .Where(s => s.PlayerUserId == playerUserId)
                .OrderBy(s => s.ItemMasterId)
                .ToListAsync();
        }

        /// <summary>
        /// 指定されたプレイヤーの特定のアイテムマスターIDのスタックアイテムを取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>スタックアイテム、存在しない場合はnull</returns>
        public async Task<StackItemEntity?> GetStackItemAsync(int playerUserId, int itemMasterId)
        {
            return await _context.StackItems
                .FirstOrDefaultAsync(s => s.PlayerUserId == playerUserId && s.ItemMasterId == itemMasterId);
        }

        /// <summary>
        /// スタックアイテムを作成する
        /// </summary>
        /// <param name="stackItem">作成するスタックアイテム</param>
        /// <returns>作成されたスタックアイテム</returns>
        public async Task<StackItemEntity> CreateStackItemAsync(StackItemEntity stackItem)
        {
            stackItem.CreatedAt = DateTime.UtcNow;
            stackItem.UpdatedAt = DateTime.UtcNow;
            
            _context.StackItems.Add(stackItem);
            await _context.SaveChangesAsync();
            return stackItem;
        }

        /// <summary>
        /// スタックアイテムを更新する
        /// </summary>
        /// <param name="stackItem">更新するスタックアイテム</param>
        /// <returns>更新されたスタックアイテム</returns>
        public async Task<StackItemEntity> UpdateStackItemAsync(StackItemEntity stackItem)
        {
            stackItem.UpdatedAt = DateTime.UtcNow;
            
            _context.StackItems.Update(stackItem);
            await _context.SaveChangesAsync();
            return stackItem;
        }

        /// <summary>
        /// スタックアイテムを削除する
        /// </summary>
        /// <param name="stackItemId">削除するスタックアイテムのID</param>
        /// <returns>削除完了を示すタスク</returns>
        public async Task DeleteStackItemAsync(int stackItemId)
        {
            var stackItem = await _context.StackItems.FindAsync(stackItemId);
            if (stackItem != null)
            {
                _context.StackItems.Remove(stackItem);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// プレイヤーが指定されたアイテムマスターIDのアイテムを持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>持っている場合はtrue、持っていない場合はfalse</returns>
        public async Task<bool> HasItemAsync(int playerUserId, int itemMasterId)
        {
            return await _context.StackItems
                .AnyAsync(s => s.PlayerUserId == playerUserId && s.ItemMasterId == itemMasterId && s.Count > 0);
        }

        /// <summary>
        /// プレイヤーが指定されたアイテムマスターIDのアイテムを指定数持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="requiredCount">必要な個数</param>
        /// <returns>必要数以上持っている場合はtrue、そうでない場合はfalse</returns>
        public async Task<bool> HasItemCountAsync(int playerUserId, int itemMasterId, int requiredCount)
        {
            var stackItem = await GetStackItemAsync(playerUserId, itemMasterId);
            return stackItem != null && stackItem.Count >= requiredCount;
        }
    }
}