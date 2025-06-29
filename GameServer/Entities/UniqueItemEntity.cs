using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ユニークアイテムを表すエンティティクラス
    /// 同じアイテムマスターIDでも別々のレコードとして管理し、個別のステータスを持つ
    /// </summary>
    [Table("unique_items")]
    public class UniqueItemEntity
    {
        /// <summary>
        /// ユニークアイテムの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueItemId { get; set; }

        /// <summary>
        /// アイテムを所有するプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int PlayerUserId { get; set; }

        /// <summary>
        /// アイテムマスターID（ItemInfoのIdと対応）
        /// </summary>
        [Required]
        public int ItemMasterId { get; set; }

        /// <summary>
        /// アイテムの強化レベル
        /// </summary>
        public int EnhancementLevel { get; set; } = 0;

        /// <summary>
        /// アイテム固有の攻撃力ボーナス
        /// </summary>
        public int AttackBonus { get; set; } = 0;

        /// <summary>
        /// アイテム固有の防御力ボーナス
        /// </summary>
        public int DefenseBonus { get; set; } = 0;

        /// <summary>
        /// アイテム固有のHP(体力)ボーナス
        /// </summary>
        public int HealthBonus { get; set; } = 0;

        /// <summary>
        /// アイテム固有のMP(マナ)ボーナス
        /// </summary>
        public int ManaBonus { get; set; } = 0;

        /// <summary>
        /// アイテムの状態（装備中、インベントリ等）
        /// </summary>
        public ItemStatus Status { get; set; } = ItemStatus.Inventory;

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
    }

    /// <summary>
    /// アイテムの状態を定義する列挙型
    /// </summary>
    public enum ItemStatus
    {
        /// <summary>
        /// インベントリに保管中
        /// </summary>
        Inventory = 0,

        /// <summary>
        /// 装備中
        /// </summary>
        Equipped = 1,

        /// <summary>
        /// 倉庫に保管中
        /// </summary>
        Storage = 2,

        /// <summary>
        /// 売却済み（削除予定）
        /// </summary>
        Sold = 3
    }
}