using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ユーザーの権限レベルを表す列挙型
    /// </summary>
    public enum UserRole
    {
        /// <summary>一般プレイヤー</summary>
        Player = 0,
        /// <summary>管理者権限</summary>
        Admin = 1
    }

    /// <summary>
    /// アカウントの状態を表す列挙型
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>アクティブ状態</summary>
        Active = 0,
        /// <summary>停止状態</summary>
        Suspended = 1,
        /// <summary>削除状態</summary>
        Deleted = 2
    }

    /// <summary>
    /// ユーザーの権限とアカウント状態を管理するエンティティクラス
    /// </summary>
    public class UserRoleEntity
    {
        /// <summary>
        /// 対象ユーザーのID
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// ユーザーの権限レベル（デフォルト：Player）
        /// </summary>
        [Required]
        public UserRole Role { get; set; } = UserRole.Player;

        /// <summary>
        /// アカウントの状態（デフォルト：Active）
        /// </summary>
        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        /// <summary>
        /// 停止処理を実行した管理者のユーザーID
        /// </summary>
        public int? SuspendedByUserId { get; set; }

        /// <summary>
        /// 停止された日時
        /// </summary>
        public DateTime? SuspendedAt { get; set; }

        /// <summary>
        /// 停止理由
        /// </summary>
        public string? SuspensionReason { get; set; }

        /// <summary>
        /// レコードの作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// レコードの最終更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 対象ユーザーのプレイヤーエンティティへの参照
        /// </summary>
        [ForeignKey("UserId")]
        public virtual PlayerEntity? Player { get; set; }

        /// <summary>
        /// 停止処理を実行した管理者のプレイヤーエンティティへの参照
        /// </summary>
        [ForeignKey("SuspendedByUserId")]
        public virtual PlayerEntity? SuspendedByUser { get; set; }
    }
}