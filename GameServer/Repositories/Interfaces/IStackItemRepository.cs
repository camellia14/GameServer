using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    /// <summary>
    /// スタックアイテムリポジトリのインターフェース
    /// スタックアイテムのデータアクセス操作を定義する
    /// </summary>
    public interface IStackItemRepository
    {
        /// <summary>
        /// 指定されたプレイヤーのスタックアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>スタックアイテムのリスト</returns>
        Task<List<StackItemEntity>> GetPlayerStackItemsAsync(int playerUserId);

        /// <summary>
        /// 指定されたプレイヤーの特定のアイテムマスターIDのスタックアイテムを取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>スタックアイテム、存在しない場合はnull</returns>
        Task<StackItemEntity?> GetStackItemAsync(int playerUserId, int itemMasterId);

        /// <summary>
        /// スタックアイテムを作成する
        /// </summary>
        /// <param name="stackItem">作成するスタックアイテム</param>
        /// <returns>作成されたスタックアイテム</returns>
        Task<StackItemEntity> CreateStackItemAsync(StackItemEntity stackItem);

        /// <summary>
        /// スタックアイテムを更新する
        /// </summary>
        /// <param name="stackItem">更新するスタックアイテム</param>
        /// <returns>更新されたスタックアイテム</returns>
        Task<StackItemEntity> UpdateStackItemAsync(StackItemEntity stackItem);

        /// <summary>
        /// スタックアイテムを削除する
        /// </summary>
        /// <param name="stackItemId">削除するスタックアイテムのID</param>
        /// <returns>削除完了を示すタスク</returns>
        Task DeleteStackItemAsync(int stackItemId);

        /// <summary>
        /// プレイヤーが指定されたアイテムマスターIDのアイテムを持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>持っている場合はtrue、持っていない場合はfalse</returns>
        Task<bool> HasItemAsync(int playerUserId, int itemMasterId);

        /// <summary>
        /// プレイヤーが指定されたアイテムマスターIDのアイテムを指定数持っているかを確認する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="requiredCount">必要な個数</param>
        /// <returns>必要数以上持っている場合はtrue、そうでない場合はfalse</returns>
        Task<bool> HasItemCountAsync(int playerUserId, int itemMasterId, int requiredCount);
    }
}