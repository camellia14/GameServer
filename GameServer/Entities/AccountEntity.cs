using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// アカウントエンティティクラス
    /// ユーザーアカウントの基本情報を管理する
    /// </summary>
    [Table("accounts")]
    public class AccountEntity
    {
        /// <summary>
        /// アカウントの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        /// <summary>
        /// ユーザー名（ログイン用）
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// パスワードハッシュ
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 表示名
        /// </summary>
        [MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// アカウントの状態
        /// </summary>
        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        /// <summary>
        /// 最後のログイン日時
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

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
        /// プレイヤーエンティティとの関連（1対1）
        /// </summary>
        public virtual PlayerEntity? Player { get; set; }

        /// <summary>
        /// プロフィールエンティティとの関連（1対1）
        /// </summary>
        public virtual ProfileEntity? Profile { get; set; }
    }

    /// <summary>
    /// アカウントの状態を定義する列挙型
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// アクティブ（通常利用可能）
        /// </summary>
        Active = 0,

        /// <summary>
        /// 停止中（管理者による停止）
        /// </summary>
        Suspended = 1,

        /// <summary>
        /// 削除済み（論理削除）
        /// </summary>
        Deleted = 2,

        /// <summary>
        /// メール未認証
        /// </summary>
        Unverified = 3,

        /// <summary>
        /// BAN（永久停止）
        /// </summary>
        Banned = 4
    }
}