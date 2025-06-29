using GameServer.MasterData.Interfaces;

namespace GameServer.MasterData
{
    /// <summary>
    /// アイテムのマスターデータ１レコード
    /// アイテムの基本情報と設定を定義する
    /// </summary>
    public class ItemInfo : IInfo
    {
        /// <summary>
        /// アイテムマスターID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// アイテムの説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// アイテムの種類
        /// 0: スタックアイテム, 1: ユニークアイテム
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// アイテムの価格（購入時）
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// アイテムの売却価格
        /// </summary>
        public int SellPrice { get; set; }

        /// <summary>
        /// スタックアイテムの場合の最大スタック数
        /// ユニークアイテムの場合は1
        /// </summary>
        public int MaxStackCount { get; set; }

        /// <summary>
        /// アイテムの効果スクリプト名（スタックアイテムで使用）
        /// </summary>
        public string? EffectScript { get; set; }

        /// <summary>
        /// アイテムのレアリティ
        /// 1: コモン, 2: アンコモン, 3: レア, 4: エピック, 5: レジェンダリー
        /// </summary>
        public int Rarity { get; set; } = 1;
    }

    /// <summary>
    /// アイテムの種類を定義する列挙型
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// スタックアイテム - 同じアイテムの個数で管理
        /// </summary>
        Stack = 0,

        /// <summary>
        /// ユニークアイテム - 個別にステータスを持つ
        /// </summary>
        Unique = 1
    }
}