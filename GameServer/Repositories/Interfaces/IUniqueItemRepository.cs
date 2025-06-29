using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    /// <summary>
    /// ユニークアイテムリポジトリのインターフェース
    /// ユニークアイテムのデータアクセス操作を定義する
    /// </summary>
    public interface IUniqueItemRepository
    {
        /// <summary>
        /// 指定されたプレイヤーのユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>ユニークアイテムのリスト</returns>
        Task<List<UniqueItemEntity>> GetPlayerUniqueItemsAsync(int playerUserId);

        /// <summary>
        /// 指定されたIDのユニークアイテムを取得する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>ユニークアイテム、存在しない場合はnull</returns>
        Task<UniqueItemEntity?> GetUniqueItemAsync(int uniqueItemId);

        /// <summary>
        /// 指定されたプレイヤーが所有する特定のアイテムマスターIDのユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>該当するユニークアイテムのリスト</returns>
        Task<List<UniqueItemEntity>> GetUniqueItemsByMasterIdAsync(int playerUserId, int itemMasterId);

        /// <summary>
        /// ユニークアイテムを作成する
        /// </summary>
        /// <param name="uniqueItem">作成するユニークアイテム</param>
        /// <returns>作成されたユニークアイテム</returns>
        Task<UniqueItemEntity> CreateUniqueItemAsync(UniqueItemEntity uniqueItem);

        /// <summary>
        /// ユニークアイテムを更新する
        /// </summary>
        /// <param name="uniqueItem">更新するユニークアイテム</param>
        /// <returns>更新されたユニークアイテム</returns>
        Task<UniqueItemEntity> UpdateUniqueItemAsync(UniqueItemEntity uniqueItem);

        /// <summary>
        /// ユニークアイテムを削除する
        /// </summary>
        /// <param name="uniqueItemId">削除するユニークアイテムのID</param>
        /// <returns>削除完了を示すタスク</returns>
        Task DeleteUniqueItemAsync(int uniqueItemId);

        /// <summary>
        /// 指定されたプレイヤーが所有する装備中のユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>装備中のユニークアイテムのリスト</returns>
        Task<List<UniqueItemEntity>> GetEquippedItemsAsync(int playerUserId);

        /// <summary>
        /// 指定されたプレイヤーが特定のアイテムマスターIDのユニークアイテムを持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>持っている場合はtrue、持っていない場合はfalse</returns>
        Task<bool> HasUniqueItemAsync(int playerUserId, int itemMasterId);
    }
}