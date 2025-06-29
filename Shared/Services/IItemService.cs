using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// アイテム関連のRPCサービスインターフェース
    /// スタックアイテムとユニークアイテムの操作を提供する
    /// </summary>
    public interface IItemService : IService<IItemService>
    {
        /// <summary>
        /// 既存のアイテム取得メソッド
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns>アイテムデータ</returns>
        UnaryResult<ItemData?> GetItem(int id);

        // スタックアイテム関連メソッド
        /// <summary>
        /// プレイヤーのスタックアイテム一覧を取得する
        /// </summary>
        /// <returns>スタックアイテムのリスト</returns>
        UnaryResult<List<StackItemData>> GetMyStackItems();

        /// <summary>
        /// スタックアイテムを使用する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="useCount">使用個数</param>
        /// <returns>使用結果</returns>
        UnaryResult<ItemUseResult> UseStackItem(int itemMasterId, int useCount);

        /// <summary>
        /// スタックアイテムを購入する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="purchaseCount">購入個数</param>
        /// <returns>購入結果</returns>
        UnaryResult<ItemPurchaseResult> PurchaseStackItem(int itemMasterId, int purchaseCount);

        /// <summary>
        /// スタックアイテムを売却する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="sellCount">売却個数</param>
        /// <returns>売却結果</returns>
        UnaryResult<ItemSellResult> SellStackItem(int itemMasterId, int sellCount);

        // ユニークアイテム関連メソッド
        /// <summary>
        /// プレイヤーのユニークアイテム一覧を取得する
        /// </summary>
        /// <returns>ユニークアイテムのリスト</returns>
        UnaryResult<List<UniqueItemData>> GetMyUniqueItems();

        /// <summary>
        /// ユニークアイテムを使用する（装備/装備解除）
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>使用結果</returns>
        UnaryResult<ItemUseResult> UseUniqueItem(int uniqueItemId);

        /// <summary>
        /// ユニークアイテムを購入する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>購入結果</returns>
        UnaryResult<ItemPurchaseResult> PurchaseUniqueItem(int itemMasterId);

        /// <summary>
        /// ユニークアイテムを売却する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>売却結果</returns>
        UnaryResult<ItemSellResult> SellUniqueItem(int uniqueItemId);

        /// <summary>
        /// ユニークアイテムを強化する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>強化結果</returns>
        UnaryResult<ItemEnhancementResult> EnhanceUniqueItem(int uniqueItemId);
    }
}
