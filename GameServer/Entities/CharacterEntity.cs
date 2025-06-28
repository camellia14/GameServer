using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ゲーム内のキャラクターを表すエンティティクラス
    /// プレイヤーが複数のキャラクターを所有できる
    /// </summary>
    public class CharacterEntity
    {
        /// <summary>
        /// キャラクターの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }

        /// <summary>
        /// このキャラクターを所有するプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int PlayerUserId { get; set; }

        /// <summary>
        /// キャラクターの名前（最大50文字）
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// キャラクターのレベル（初期値は1）
        /// </summary>
        [Required]
        public int Level { get; set; } = 1;

        /// <summary>
        /// キャラクターのX座標位置
        /// </summary>
        [Required]
        public float PositionX { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターのY座標位置
        /// </summary>
        [Required]
        public float PositionY { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターの作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// キャラクターの最終更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// このキャラクターを所有するプレイヤーエンティティへの参照
        /// </summary>
        [ForeignKey("PlayerUserId")]
        public virtual PlayerEntity? Player { get; set; }
    }
}