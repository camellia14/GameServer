using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// スタックアイテムを表すエンティティクラス
    /// 同じアイテムマスターIDのアイテムを個数で管理する
    /// </summary>
    [Table("stack_items")]
    public class StackItemEntity
    {
        /// <summary>
        /// スタックアイテムの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StackItemId { get; set; }

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
        /// 保有個数
        /// </summary>
        [Required]
        public int Count { get; set; }

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
}