using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// クエストエンティティクラス
    /// プレイヤーのクエスト進捗情報を管理する
    /// </summary>
    [Table("quests")]
    public class QuestEntity
    {
        /// <summary>
        /// クエストの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestId { get; set; }

        /// <summary>
        /// クエストを受けているプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int PlayerUserId { get; set; }

        /// <summary>
        /// クエストマスターID
        /// </summary>
        [Required]
        public int QuestMasterId { get; set; }

        /// <summary>
        /// クエストの状態
        /// </summary>
        [Required]
        public QuestStatus Status { get; set; } = QuestStatus.InProgress;

        /// <summary>
        /// 現在の進捗値
        /// </summary>
        [Required]
        public int CurrentProgress { get; set; } = 0;

        /// <summary>
        /// 目標進捗値
        /// </summary>
        [Required]
        public int TargetProgress { get; set; } = 1;

        /// <summary>
        /// クエスト開始日時
        /// </summary>
        [Required]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// クエスト完了日時
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 報酬受取日時
        /// </summary>
        public DateTime? RewardClaimedAt { get; set; }

        /// <summary>
        /// クエストの期限日時（無期限の場合はnull）
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// レコード作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// レコード更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(PlayerUserId))]
        public virtual PlayerEntity? Player { get; set; }

        /// <summary>
        /// クエストが完了しているかを判定する
        /// </summary>
        /// <returns>完了している場合はtrue</returns>
        public bool IsCompleted()
        {
            return Status == QuestStatus.Completed || Status == QuestStatus.RewardClaimed;
        }

        /// <summary>
        /// クエストが期限切れかを判定する
        /// </summary>
        /// <returns>期限切れの場合はtrue</returns>
        public bool IsExpired()
        {
            return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        }

        /// <summary>
        /// 進捗率を取得する（0.0-1.0）
        /// </summary>
        /// <returns>進捗率</returns>
        public float GetProgressRatio()
        {
            if (TargetProgress <= 0) return 1.0f;
            return Math.Min(1.0f, (float)CurrentProgress / TargetProgress);
        }

        /// <summary>
        /// クエストの進捗を更新する
        /// </summary>
        /// <param name="progress">追加する進捗値</param>
        /// <returns>目標達成した場合はtrue</returns>
        public bool AddProgress(int progress)
        {
            CurrentProgress = Math.Min(CurrentProgress + progress, TargetProgress);
            UpdatedAt = DateTime.UtcNow;

            if (CurrentProgress >= TargetProgress && Status == QuestStatus.InProgress)
            {
                Status = QuestStatus.Completed;
                CompletedAt = DateTime.UtcNow;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 報酬を受け取る
        /// </summary>
        /// <returns>報酬受け取りに成功した場合はtrue</returns>
        public bool ClaimReward()
        {
            if (Status != QuestStatus.Completed) return false;

            Status = QuestStatus.RewardClaimed;
            RewardClaimedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }
    }

    /// <summary>
    /// クエストの状態を定義する列挙型
    /// </summary>
    public enum QuestStatus
    {
        /// <summary>
        /// 進行中
        /// </summary>
        InProgress = 0,

        /// <summary>
        /// 完了（報酬未受取）
        /// </summary>
        Completed = 1,

        /// <summary>
        /// 報酬受取済み
        /// </summary>
        RewardClaimed = 2,

        /// <summary>
        /// 放棄
        /// </summary>
        Abandoned = 3,

        /// <summary>
        /// 期限切れ
        /// </summary>
        Expired = 4
    }
}