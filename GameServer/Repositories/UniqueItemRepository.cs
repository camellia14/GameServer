using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    /// <summary>
    /// ユニークアイテムリポジトリの実装クラス
    /// ユニークアイテムのデータアクセス操作を提供する
    /// </summary>
    public class UniqueItemRepository : IUniqueItemRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// UniqueItemRepositoryのコンストラクタ
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public UniqueItemRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 指定されたプレイヤーのユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>ユニークアイテムのリスト</returns>
        public async Task<List<UniqueItemEntity>> GetPlayerUniqueItemsAsync(int playerUserId)
        {
            return await _context.UniqueItems
                .Where(u => u.PlayerUserId == playerUserId && u.Status != ItemStatus.Sold)
                .OrderBy(u => u.ItemMasterId)
                .ThenBy(u => u.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// 指定されたIDのユニークアイテムを取得する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>ユニークアイテム、存在しない場合はnull</returns>
        public async Task<UniqueItemEntity?> GetUniqueItemAsync(int uniqueItemId)
        {
            return await _context.UniqueItems
                .FirstOrDefaultAsync(u => u.UniqueItemId == uniqueItemId && u.Status != ItemStatus.Sold);
        }

        /// <summary>
        /// 指定されたプレイヤーが所有する特定のアイテムマスターIDのユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>該当するユニークアイテムのリスト</returns>
        public async Task<List<UniqueItemEntity>> GetUniqueItemsByMasterIdAsync(int playerUserId, int itemMasterId)
        {
            return await _context.UniqueItems
                .Where(u => u.PlayerUserId == playerUserId && u.ItemMasterId == itemMasterId && u.Status != ItemStatus.Sold)
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// ユニークアイテムを作成する
        /// </summary>
        /// <param name="uniqueItem">作成するユニークアイテム</param>
        /// <returns>作成されたユニークアイテム</returns>
        public async Task<UniqueItemEntity> CreateUniqueItemAsync(UniqueItemEntity uniqueItem)
        {
            uniqueItem.CreatedAt = DateTime.UtcNow;
            uniqueItem.UpdatedAt = DateTime.UtcNow;
            
            _context.UniqueItems.Add(uniqueItem);
            await _context.SaveChangesAsync();
            return uniqueItem;
        }

        /// <summary>
        /// ユニークアイテムを更新する
        /// </summary>
        /// <param name="uniqueItem">更新するユニークアイテム</param>
        /// <returns>更新されたユニークアイテム</returns>
        public async Task<UniqueItemEntity> UpdateUniqueItemAsync(UniqueItemEntity uniqueItem)
        {
            uniqueItem.UpdatedAt = DateTime.UtcNow;
            
            _context.UniqueItems.Update(uniqueItem);
            await _context.SaveChangesAsync();
            return uniqueItem;
        }

        /// <summary>
        /// ユニークアイテムを削除する
        /// </summary>
        /// <param name="uniqueItemId">削除するユニークアイテムのID</param>
        /// <returns>削除完了を示すタスク</returns>
        public async Task DeleteUniqueItemAsync(int uniqueItemId)
        {
            var uniqueItem = await _context.UniqueItems.FindAsync(uniqueItemId);
            if (uniqueItem != null)
            {
                // 物理削除ではなく売却済み状態に変更
                uniqueItem.Status = ItemStatus.Sold;
                uniqueItem.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 指定されたプレイヤーが所有する装備中のユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>装備中のユニークアイテムのリスト</returns>
        public async Task<List<UniqueItemEntity>> GetEquippedItemsAsync(int playerUserId)
        {
            return await _context.UniqueItems
                .Where(u => u.PlayerUserId == playerUserId && u.Status == ItemStatus.Equipped)
                .OrderBy(u => u.ItemMasterId)
                .ToListAsync();
        }

        /// <summary>
        /// 指定されたプレイヤーが特定のアイテムマスターIDのユニークアイテムを持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>持っている場合はtrue、持っていない場合はfalse</returns>
        public async Task<bool> HasUniqueItemAsync(int playerUserId, int itemMasterId)
        {
            return await _context.UniqueItems
                .AnyAsync(u => u.PlayerUserId == playerUserId && u.ItemMasterId == itemMasterId && u.Status != ItemStatus.Sold);
        }
    }
}