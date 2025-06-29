using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// フレンドエンティティクラス
    /// プレイヤー間のフレンド関係を管理する
    /// </summary>
    [Table("friends")]
    public class FriendEntity
    {
        /// <summary>
        /// フレンド関係の一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }

        /// <summary>
        /// フレンド申請を送信したプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int RequesterUserId { get; set; }

        /// <summary>
        /// フレンド申請を受信したプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int AddresseeUserId { get; set; }

        /// <summary>
        /// フレンド関係の状態
        /// </summary>
        [Required]
        public FriendStatus Status { get; set; } = FriendStatus.Pending;

        /// <summary>
        /// フレンド申請の送信日時
        /// </summary>
        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// フレンド申請の承認・拒否日時
        /// </summary>
        public DateTime? RespondedAt { get; set; }

        /// <summary>
        /// フレンド申請時のメッセージ
        /// </summary>
        [MaxLength(500)]
        public string RequestMessage { get; set; } = string.Empty;

        /// <summary>
        /// 最後に一緒にプレイした日時
        /// </summary>
        public DateTime? LastPlayedTogetherAt { get; set; }

        /// <summary>
        /// フレンドのお気に入り設定
        /// </summary>
        public bool IsFavorite { get; set; } = false;

        /// <summary>
        /// フレンドをブロックしているかどうか
        /// </summary>
        public bool IsBlocked { get; set; } = false;

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
        /// 申請者プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(RequesterUserId))]
        public virtual PlayerEntity? RequesterPlayer { get; set; }

        /// <summary>
        /// 受信者プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(AddresseeUserId))]
        public virtual PlayerEntity? AddresseePlayer { get; set; }

        /// <summary>
        /// フレンド申請を承認する
        /// </summary>
        /// <returns>承認に成功した場合はtrue</returns>
        public bool Accept()
        {
            if (Status != FriendStatus.Pending) return false;

            Status = FriendStatus.Accepted;
            RespondedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// フレンド申請を拒否する
        /// </summary>
        /// <returns>拒否に成功した場合はtrue</returns>
        public bool Reject()
        {
            if (Status != FriendStatus.Pending) return false;

            Status = FriendStatus.Rejected;
            RespondedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// フレンド関係を削除する
        /// </summary>
        /// <returns>削除に成功した場合はtrue</returns>
        public bool Remove()
        {
            if (Status != FriendStatus.Accepted) return false;

            Status = FriendStatus.Removed;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// フレンドをブロックする
        /// </summary>
        public void Block()
        {
            IsBlocked = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// フレンドのブロックを解除する
        /// </summary>
        public void Unblock()
        {
            IsBlocked = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// お気に入り設定を切り替える
        /// </summary>
        public void ToggleFavorite()
        {
            IsFavorite = !IsFavorite;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 一緒にプレイした記録を更新する
        /// </summary>
        public void UpdateLastPlayedTogether()
        {
            LastPlayedTogetherAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 指定されたユーザーIDから見た相手のユーザーIDを取得する
        /// </summary>
        /// <param name="viewerUserId">閲覧者のユーザーID</param>
        /// <returns>相手のユーザーID</returns>
        public int GetOtherUserId(int viewerUserId)
        {
            return viewerUserId == RequesterUserId ? AddresseeUserId : RequesterUserId;
        }

        /// <summary>
        /// 指定されたユーザーIDが申請者かどうかを判定する
        /// </summary>
        /// <param name="userId">判定するユーザーID</param>
        /// <returns>申請者の場合はtrue</returns>
        public bool IsRequester(int userId)
        {
            return RequesterUserId == userId;
        }
    }

    /// <summary>
    /// フレンド関係の状態を定義する列挙型
    /// </summary>
    public enum FriendStatus
    {
        /// <summary>
        /// 申請中（承認待ち）
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 承認済み（フレンド）
        /// </summary>
        Accepted = 1,

        /// <summary>
        /// 拒否済み
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// 削除済み
        /// </summary>
        Removed = 3
    }
}