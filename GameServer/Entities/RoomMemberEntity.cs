using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ルームメンバーエンティティクラス
    /// ルーム参加者の情報を管理する
    /// </summary>
    [Table("room_members")]
    public class RoomMemberEntity
    {
        /// <summary>
        /// ルームメンバーの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomMemberId { get; set; }

        /// <summary>
        /// ルームID
        /// </summary>
        [Required]
        public int RoomId { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// メンバーの役割
        /// </summary>
        [Required]
        public RoomMemberRole Role { get; set; } = RoomMemberRole.Member;

        /// <summary>
        /// ルーム参加日時
        /// </summary>
        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ルーム退出日時（退出した場合）
        /// </summary>
        public DateTime? LeftAt { get; set; }

        /// <summary>
        /// メンバーの状態
        /// </summary>
        [Required]
        public RoomMemberStatus Status { get; set; } = RoomMemberStatus.Active;

        /// <summary>
        /// BANされた日時（BANされた場合）
        /// </summary>
        public DateTime? BannedAt { get; set; }

        /// <summary>
        /// BANした管理者のユーザーID
        /// </summary>
        public int? BannedByUserId { get; set; }

        /// <summary>
        /// BAN理由
        /// </summary>
        [MaxLength(200)]
        public string? BanReason { get; set; }

        /// <summary>
        /// 招待した管理者のユーザーID
        /// </summary>
        public int? InvitedByUserId { get; set; }

        /// <summary>
        /// 最後のアクティビティ日時
        /// </summary>
        public DateTime? LastActivityAt { get; set; }

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
        /// ルームエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(RoomId))]
        public virtual RoomEntity? Room { get; set; }

        /// <summary>
        /// プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual PlayerEntity? User { get; set; }

        /// <summary>
        /// 招待者プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(InvitedByUserId))]
        public virtual PlayerEntity? InvitedByUser { get; set; }

        /// <summary>
        /// BAN実行者プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(BannedByUserId))]
        public virtual PlayerEntity? BannedByUser { get; set; }

        /// <summary>
        /// メンバーをルームから退出させる
        /// </summary>
        public void Leave()
        {
            if (Status == RoomMemberStatus.Active)
            {
                Status = RoomMemberStatus.Left;
                LeftAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メンバーをBANする
        /// </summary>
        /// <param name="bannedByUserId">BAN実行者のユーザーID</param>
        /// <param name="reason">BAN理由</param>
        public void Ban(int bannedByUserId, string? reason = null)
        {
            if (Status == RoomMemberStatus.Active)
            {
                Status = RoomMemberStatus.Banned;
                BannedAt = DateTime.UtcNow;
                BannedByUserId = bannedByUserId;
                BanReason = reason;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メンバーのBANを解除する
        /// </summary>
        public void Unban()
        {
            if (Status == RoomMemberStatus.Banned)
            {
                Status = RoomMemberStatus.Active;
                BannedAt = null;
                BannedByUserId = null;
                BanReason = null;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メンバーをキックする（一時的な追い出し）
        /// </summary>
        public void Kick()
        {
            if (Status == RoomMemberStatus.Active)
            {
                Status = RoomMemberStatus.Kicked;
                LeftAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メンバーの役割を変更する
        /// </summary>
        /// <param name="newRole">新しい役割</param>
        public void ChangeRole(RoomMemberRole newRole)
        {
            if (Status == RoomMemberStatus.Active && Role != newRole)
            {
                Role = newRole;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 最後のアクティビティ時間を更新する
        /// </summary>
        public void UpdateActivity()
        {
            LastActivityAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// メンバーがアクティブかを判定する
        /// </summary>
        /// <returns>アクティブな場合はtrue</returns>
        public bool IsActive()
        {
            return Status == RoomMemberStatus.Active;
        }

        /// <summary>
        /// メンバーが管理者権限を持つかを判定する
        /// </summary>
        /// <returns>管理者権限を持つ場合はtrue</returns>
        public bool HasAdminRights()
        {
            return Role == RoomMemberRole.Owner || Role == RoomMemberRole.Admin;
        }

        /// <summary>
        /// メンバーがモデレーター権限を持つかを判定する
        /// </summary>
        /// <returns>モデレーター権限を持つ場合はtrue</returns>
        public bool HasModeratorRights()
        {
            return HasAdminRights() || Role == RoomMemberRole.Moderator;
        }
    }

    /// <summary>
    /// ルームメンバーの役割を定義する列挙型
    /// </summary>
    public enum RoomMemberRole
    {
        /// <summary>
        /// 一般メンバー
        /// </summary>
        Member = 0,

        /// <summary>
        /// モデレーター
        /// </summary>
        Moderator = 1,

        /// <summary>
        /// 管理者
        /// </summary>
        Admin = 2,

        /// <summary>
        /// オーナー
        /// </summary>
        Owner = 3
    }

    /// <summary>
    /// ルームメンバーの状態を定義する列挙型
    /// </summary>
    public enum RoomMemberStatus
    {
        /// <summary>
        /// アクティブ（参加中）
        /// </summary>
        Active = 0,

        /// <summary>
        /// 退出済み
        /// </summary>
        Left = 1,

        /// <summary>
        /// キック済み
        /// </summary>
        Kicked = 2,

        /// <summary>
        /// BAN済み
        /// </summary>
        Banned = 3
    }
}