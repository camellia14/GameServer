using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// 既存のアイテムデータクラス
    /// </summary>
    public class ItemData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// スタックアイテムのデータクラス
    /// </summary>
    [MessagePackObject]
    public class StackItemData
    {
        /// <summary>
        /// スタックアイテムID
        /// </summary>
        [Key(0)]
        public int StackItemId { get; set; }

        /// <summary>
        /// アイテムマスターID
        /// </summary>
        [Key(1)]
        public int ItemMasterId { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [Key(2)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 所有個数
        /// </summary>
        [Key(3)]
        public int Count { get; set; }

        /// <summary>
        /// 最大スタック数
        /// </summary>
        [Key(4)]
        public int MaxStackCount { get; set; }
    }

    /// <summary>
    /// ユニークアイテムのデータクラス
    /// </summary>
    [MessagePackObject]
    public class UniqueItemData
    {
        /// <summary>
        /// ユニークアイテムID
        /// </summary>
        [Key(0)]
        public int UniqueItemId { get; set; }

        /// <summary>
        /// アイテムマスターID
        /// </summary>
        [Key(1)]
        public int ItemMasterId { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [Key(2)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 強化レベル
        /// </summary>
        [Key(3)]
        public int EnhancementLevel { get; set; }

        /// <summary>
        /// 攻撃力ボーナス
        /// </summary>
        [Key(4)]
        public int AttackBonus { get; set; }

        /// <summary>
        /// 防御力ボーナス
        /// </summary>
        [Key(5)]
        public int DefenseBonus { get; set; }

        /// <summary>
        /// HP(体力)ボーナス
        /// </summary>
        [Key(6)]
        public int HealthBonus { get; set; }

        /// <summary>
        /// MP(マナ)ボーナス
        /// </summary>
        [Key(7)]
        public int ManaBonus { get; set; }

        /// <summary>
        /// アイテムの状態
        /// </summary>
        [Key(8)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 装備中かどうか
        /// </summary>
        [Key(9)]
        public bool IsEquipped { get; set; }
    }

    /// <summary>
    /// アイテム使用結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ItemUseResult
    {
        /// <summary>
        /// 使用が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 残り個数（スタックアイテムの場合）
        /// </summary>
        [Key(2)]
        public int RemainingCount { get; set; }
    }

    /// <summary>
    /// アイテム購入結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ItemPurchaseResult
    {
        /// <summary>
        /// 購入が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 購入総額
        /// </summary>
        [Key(2)]
        public int TotalPrice { get; set; }

        /// <summary>
        /// 残り所持金
        /// </summary>
        [Key(3)]
        public int RemainingMoney { get; set; }
    }

    /// <summary>
    /// アイテム売却結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ItemSellResult
    {
        /// <summary>
        /// 売却が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 売却総額
        /// </summary>
        [Key(2)]
        public int TotalSellPrice { get; set; }

        /// <summary>
        /// 残り所持金
        /// </summary>
        [Key(3)]
        public int RemainingMoney { get; set; }

        /// <summary>
        /// 残り個数（スタックアイテムの場合）
        /// </summary>
        [Key(4)]
        public int RemainingCount { get; set; }
    }

    /// <summary>
    /// アイテム強化結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ItemEnhancementResult
    {
        /// <summary>
        /// 強化が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 新しい強化レベル
        /// </summary>
        [Key(2)]
        public int NewEnhancementLevel { get; set; }

        /// <summary>
        /// 強化にかかった費用
        /// </summary>
        [Key(3)]
        public int EnhancementCost { get; set; }

        /// <summary>
        /// 残り所持金
        /// </summary>
        [Key(4)]
        public int RemainingMoney { get; set; }
    }
}