using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// クエスト情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class QuestData
    {
        /// <summary>
        /// クエストID
        /// </summary>
        [Key(0)]
        public int QuestId { get; set; }

        /// <summary>
        /// クエストマスターID
        /// </summary>
        [Key(1)]
        public int QuestMasterId { get; set; }

        /// <summary>
        /// クエスト名
        /// </summary>
        [Key(2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// クエストの説明
        /// </summary>
        [Key(3)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// クエストの種類
        /// </summary>
        [Key(4)]
        public string QuestType { get; set; } = string.Empty;

        /// <summary>
        /// クエストの状態
        /// </summary>
        [Key(5)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 現在の進捗値
        /// </summary>
        [Key(6)]
        public int CurrentProgress { get; set; }

        /// <summary>
        /// 目標進捗値
        /// </summary>
        [Key(7)]
        public int TargetProgress { get; set; }

        /// <summary>
        /// 進捗率（0.0-1.0）
        /// </summary>
        [Key(8)]
        public float ProgressRatio { get; set; }

        /// <summary>
        /// 難易度
        /// </summary>
        [Key(9)]
        public string Difficulty { get; set; } = string.Empty;

        /// <summary>
        /// 経験値報酬
        /// </summary>
        [Key(10)]
        public long ExperienceReward { get; set; }

        /// <summary>
        /// お金報酬
        /// </summary>
        [Key(11)]
        public int MoneyReward { get; set; }

        /// <summary>
        /// アイテム報酬があるかどうか
        /// </summary>
        [Key(12)]
        public bool HasItemReward { get; set; }

        /// <summary>
        /// アイテム報酬の名前
        /// </summary>
        [Key(13)]
        public string ItemRewardName { get; set; } = string.Empty;

        /// <summary>
        /// アイテム報酬の個数
        /// </summary>
        [Key(14)]
        public int ItemRewardCount { get; set; }

        /// <summary>
        /// クエスト開始日時
        /// </summary>
        [Key(15)]
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// クエスト完了日時
        /// </summary>
        [Key(16)]
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 期限日時
        /// </summary>
        [Key(17)]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 繰り返し可能かどうか
        /// </summary>
        [Key(18)]
        public bool IsRepeatable { get; set; }
    }

    /// <summary>
    /// クエスト開始リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class QuestStartRequest
    {
        /// <summary>
        /// 開始するクエストのマスターID
        /// </summary>
        [Key(0)]
        public int QuestMasterId { get; set; }
    }

    /// <summary>
    /// クエスト進捗更新リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class QuestProgressRequest
    {
        /// <summary>
        /// 更新するクエストID
        /// </summary>
        [Key(0)]
        public int QuestId { get; set; }

        /// <summary>
        /// 追加する進捗値
        /// </summary>
        [Key(1)]
        public int Progress { get; set; }
    }

    /// <summary>
    /// クエスト報酬クレームリクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class QuestRewardClaimRequest
    {
        /// <summary>
        /// 報酬を受け取るクエストID
        /// </summary>
        [Key(0)]
        public int QuestId { get; set; }
    }

    /// <summary>
    /// クエスト報酬結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class QuestRewardResult
    {
        /// <summary>
        /// 処理が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 獲得した経験値
        /// </summary>
        [Key(2)]
        public long ExperienceGained { get; set; }

        /// <summary>
        /// 獲得したお金
        /// </summary>
        [Key(3)]
        public int MoneyGained { get; set; }

        /// <summary>
        /// 獲得したアイテムのリスト
        /// </summary>
        [Key(4)]
        public List<RewardItem> ItemsGained { get; set; } = new();

        /// <summary>
        /// レベルアップしたかどうか
        /// </summary>
        [Key(5)]
        public bool LeveledUp { get; set; }

        /// <summary>
        /// 新しいレベル
        /// </summary>
        [Key(6)]
        public int NewLevel { get; set; }
    }

    /// <summary>
    /// 報酬アイテムのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RewardItem
    {
        /// <summary>
        /// アイテムマスターID
        /// </summary>
        [Key(0)]
        public int ItemMasterId { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [Key(1)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 獲得個数
        /// </summary>
        [Key(2)]
        public int Count { get; set; }
    }
}