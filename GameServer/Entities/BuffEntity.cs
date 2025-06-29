using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// バフエンティティクラス
    /// キャラクターに付与されるバフの状態を管理する
    /// </summary>
    [Table("buffs")]
    public class BuffEntity
    {
        /// <summary>
        /// バフの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuffId { get; set; }

        /// <summary>
        /// バフを受けているキャラクターのID
        /// </summary>
        [Required]
        public int CharacterId { get; set; }

        /// <summary>
        /// バフマスターID（バフの種類を識別）
        /// </summary>
        [Required]
        public int BuffMasterId { get; set; }

        /// <summary>
        /// バフのレベル（強度）
        /// </summary>
        [Required]
        public int BuffLevel { get; set; } = 1;

        /// <summary>
        /// バフのタイプ
        /// </summary>
        [Required]
        public BuffType BuffType { get; set; }

        /// <summary>
        /// バフのスタック数
        /// </summary>
        [Required]
        public int StackCount { get; set; } = 1;

        /// <summary>
        /// 効果開始時間（UTC）
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 効果終了時間（UTC）、永続効果の場合はnull
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 効果継続時間（秒）、永続効果の場合は-1
        /// </summary>
        public int DurationSeconds { get; set; } = -1;

        /// <summary>
        /// 次回スタック減少時間（UTC）、スタック減少しない場合はnull
        /// </summary>
        public DateTime? NextStackDecreaseTime { get; set; }

        /// <summary>
        /// スタック減少間隔（秒）、スタック減少しない場合は0
        /// </summary>
        public int StackDecreaseIntervalSeconds { get; set; } = 0;

        /// <summary>
        /// バフが有効かどうか
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

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
        /// キャラクターエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public virtual CharacterEntity? Character { get; set; }

        /// <summary>
        /// バフが永続効果かどうかを判定する
        /// </summary>
        /// <returns>永続効果の場合はtrue</returns>
        public bool IsPermanent()
        {
            return DurationSeconds == -1 || EndTime == null;
        }

        /// <summary>
        /// バフが期限切れかどうかを判定する
        /// </summary>
        /// <returns>期限切れの場合はtrue</returns>
        public bool IsExpired()
        {
            if (IsPermanent()) return false;
            return EndTime.HasValue && DateTime.UtcNow > EndTime.Value;
        }

        /// <summary>
        /// スタック減少のタイミングかどうかを判定する
        /// </summary>
        /// <returns>スタック減少のタイミングの場合はtrue</returns>
        public bool ShouldDecreaseStack()
        {
            if (StackDecreaseIntervalSeconds <= 0) return false;
            return NextStackDecreaseTime.HasValue && DateTime.UtcNow >= NextStackDecreaseTime.Value;
        }
    }

    /// <summary>
    /// バフのタイプを定義する列挙型
    /// </summary>
    public enum BuffType
    {
        /// <summary>
        /// 有利効果（バフ）
        /// </summary>
        Buff = 0,

        /// <summary>
        /// 不利効果（デバフ）
        /// </summary>
        Debuff = 1,

        /// <summary>
        /// 状態異常
        /// </summary>
        Abnormal = 2
    }
}