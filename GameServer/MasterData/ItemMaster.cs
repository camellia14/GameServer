using GameServer.MasterData.Interfaces;

namespace GameServer.MasterData
{
    /// <summary>
    /// アイテムマスターデータクラス
    /// 全アイテムの基本情報を管理する
    /// </summary>
    public class ItemMaster : BaseMaster<ItemInfo>
    {
        /// <summary>
        /// 指定されたアイテムタイプのアイテム一覧を取得する
        /// </summary>
        /// <param name="itemType">取得したいアイテムの種類</param>
        /// <returns>指定された種類のアイテムリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetByType(ItemType itemType)
        {
            return GetAll().Where(item => item.ItemType == itemType).ToList().AsReadOnly();
        }

        /// <summary>
        /// スタックアイテム一覧を取得する
        /// </summary>
        /// <returns>スタックアイテムのリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetStackItems()
        {
            return GetByType(ItemType.Stack);
        }

        /// <summary>
        /// ユニークアイテム一覧を取得する
        /// </summary>
        /// <returns>ユニークアイテムのリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetUniqueItems()
        {
            return GetByType(ItemType.Unique);
        }

        /// <summary>
        /// 指定されたレアリティのアイテム一覧を取得する
        /// </summary>
        /// <param name="rarity">取得したいレアリティ（1-5）</param>
        /// <returns>指定されたレアリティのアイテムリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetByRarity(int rarity)
        {
            return GetAll().Where(item => item.Rarity == rarity).ToList().AsReadOnly();
        }

        /// <summary>
        /// 購入可能なアイテム一覧を取得する（価格が0より大きい）
        /// </summary>
        /// <returns>購入可能なアイテムリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetPurchasableItems()
        {
            return GetAll().Where(item => item.Price > 0).ToList().AsReadOnly();
        }

        /// <summary>
        /// 売却可能なアイテム一覧を取得する（売却価格が0より大きい）
        /// </summary>
        /// <returns>売却可能なアイテムリスト</returns>
        public IReadOnlyCollection<ItemInfo> GetSellableItems()
        {
            return GetAll().Where(item => item.SellPrice > 0).ToList().AsReadOnly();
        }
    }
}