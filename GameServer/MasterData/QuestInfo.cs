using GameServer.MasterData.Interfaces;

namespace GameServer.MasterData
{
    /// <summary>
    /// クエストのマスターデータ１レコード
    /// クエストの基本情報と設定を定義する
    /// </summary>
    public class QuestInfo : IInfo
    {
        /// <summary>
        /// クエストマスターID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// クエスト名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// クエストの説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// クエストの種類
        /// </summary>
        public QuestType QuestType { get; set; }

        /// <summary>
        /// 目標進捗値
        /// </summary>
        public int TargetProgress { get; set; } = 1;

        /// <summary>
        /// クエストの難易度
        /// </summary>
        public QuestDifficulty Difficulty { get; set; } = QuestDifficulty.Easy;

        /// <summary>
        /// 制限時間（秒）、無制限の場合は-1
        /// </summary>
        public int TimeLimitSeconds { get; set; } = -1;

        /// <summary>
        /// 前提条件となるクエストID（なしの場合は0）
        /// </summary>
        public int PrerequisiteQuestId { get; set; } = 0;

        /// <summary>
        /// 必要プレイヤーレベル
        /// </summary>
        public int RequiredLevel { get; set; } = 1;

        /// <summary>
        /// 経験値報酬
        /// </summary>
        public long ExperienceReward { get; set; } = 0;

        /// <summary>
        /// お金報酬
        /// </summary>
        public int MoneyReward { get; set; } = 0;

        /// <summary>
        /// アイテム報酬のマスターID（なしの場合は0）
        /// </summary>
        public int ItemRewardMasterId { get; set; } = 0;

        /// <summary>
        /// アイテム報酬の個数
        /// </summary>
        public int ItemRewardCount { get; set; } = 0;

        /// <summary>
        /// クエストが繰り返し可能かどうか
        /// </summary>
        public bool IsRepeatable { get; set; } = false;

        /// <summary>
        /// 繰り返し間隔（時間）、繰り返し不可の場合は0
        /// </summary>
        public int RepeatIntervalHours { get; set; } = 0;

        /// <summary>
        /// クエストの優先度（表示順序用）
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// クエストが有効かどうか
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 制限時間があるクエストかどうかを判定する
        /// </summary>
        /// <returns>制限時間がある場合はtrue</returns>
        public bool HasTimeLimit()
        {
            return TimeLimitSeconds > 0;
        }

        /// <summary>
        /// 前提条件があるクエストかどうかを判定する
        /// </summary>
        /// <returns>前提条件がある場合はtrue</returns>
        public bool HasPrerequisite()
        {
            return PrerequisiteQuestId > 0;
        }

        /// <summary>
        /// アイテム報酬があるかどうかを判定する
        /// </summary>
        /// <returns>アイテム報酬がある場合はtrue</returns>
        public bool HasItemReward()
        {
            return ItemRewardMasterId > 0 && ItemRewardCount > 0;
        }
    }

    /// <summary>
    /// クエストの種類を定義する列挙型
    /// </summary>
    public enum QuestType
    {
        /// <summary>
        /// メインクエスト
        /// </summary>
        Main = 0,

        /// <summary>
        /// サブクエスト
        /// </summary>
        Sub = 1,

        /// <summary>
        /// デイリークエスト
        /// </summary>
        Daily = 2,

        /// <summary>
        /// ウィークリークエスト
        /// </summary>
        Weekly = 3,

        /// <summary>
        /// 実績クエスト
        /// </summary>
        Achievement = 4,

        /// <summary>
        /// イベントクエスト
        /// </summary>
        Event = 5
    }

    /// <summary>
    /// クエストの難易度を定義する列挙型
    /// </summary>
    public enum QuestDifficulty
    {
        /// <summary>
        /// 簡単
        /// </summary>
        Easy = 0,

        /// <summary>
        /// 普通
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 難しい
        /// </summary>
        Hard = 2,

        /// <summary>
        /// 非常に難しい
        /// </summary>
        VeryHard = 3,

        /// <summary>
        /// 極めて難しい
        /// </summary>
        Extreme = 4
    }
}