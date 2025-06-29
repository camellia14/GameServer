using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ルームエンティティクラス
    /// ゲームルームの情報を管理する
    /// </summary>
    [Table("rooms")]
    public class RoomEntity
    {
        /// <summary>
        /// ルームの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        /// <summary>
        /// ルーム名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// ルームの説明
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ルームを作成したユーザーID（オーナー）
        /// </summary>
        [Required]
        public int OwnerUserId { get; set; }

        /// <summary>
        /// ルームのパスワード（任意）
        /// </summary>
        [MaxLength(50)]
        public string? Password { get; set; }

        /// <summary>
        /// ルームの最大参加者数
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int MaxParticipants { get; set; } = 10;

        /// <summary>
        /// 現在の参加者数
        /// </summary>
        public int CurrentParticipants { get; set; } = 0;

        /// <summary>
        /// ルームの状態
        /// </summary>
        [Required]
        public RoomStatus Status { get; set; } = RoomStatus.Active;

        /// <summary>
        /// ルームのタイプ
        /// </summary>
        [Required]
        public RoomType RoomType { get; set; } = RoomType.Public;

        /// <summary>
        /// プライベートルームかどうか
        /// </summary>
        public bool IsPrivate { get; set; } = false;

        /// <summary>
        /// フレンドのみ参加可能かどうか
        /// </summary>
        public bool IsFriendsOnly { get; set; } = false;

        /// <summary>
        /// ルームが削除されたかどうか（論理削除）
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// ルーム作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ルーム更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後のアクティビティ日時
        /// </summary>
        public DateTime? LastActivityAt { get; set; }

        /// <summary>
        /// オーナープレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(OwnerUserId))]
        public virtual PlayerEntity? Owner { get; set; }

        /// <summary>
        /// パスワード保護されているかを判定する
        /// </summary>
        /// <returns>パスワード保護されている場合はtrue</returns>
        public bool IsPasswordProtected()
        {
            return !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// ルームが満員かを判定する
        /// </summary>
        /// <returns>満員の場合はtrue</returns>
        public bool IsFull()
        {
            return CurrentParticipants >= MaxParticipants;
        }

        /// <summary>
        /// ルームが参加可能かを判定する
        /// </summary>
        /// <returns>参加可能な場合はtrue</returns>
        public bool IsJoinable()
        {
            return Status == RoomStatus.Active && !IsFull() && !IsDeleted;
        }

        /// <summary>
        /// パスワードが正しいかを検証する
        /// </summary>
        /// <param name="password">検証するパスワード</param>
        /// <returns>正しい場合はtrue</returns>
        public bool VerifyPassword(string? password)
        {
            if (!IsPasswordProtected())
                return true;

            return Password == password;
        }

        /// <summary>
        /// ルーム情報を更新する
        /// </summary>
        /// <param name="roomName">新しいルーム名</param>
        /// <param name="description">新しい説明</param>
        /// <param name="maxParticipants">新しい最大参加者数</param>
        /// <param name="password">新しいパスワード</param>
        public void UpdateRoomInfo(string? roomName = null, string? description = null, int? maxParticipants = null, string? password = null)
        {
            if (!string.IsNullOrWhiteSpace(roomName))
                RoomName = roomName.Trim();

            if (description != null)
                Description = description.Trim();

            if (maxParticipants.HasValue && maxParticipants.Value >= CurrentParticipants)
                MaxParticipants = maxParticipants.Value;

            if (password != null)
                Password = string.IsNullOrWhiteSpace(password) ? null : password;

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 参加者数を増加させる
        /// </summary>
        /// <returns>増加に成功した場合はtrue</returns>
        public bool IncrementParticipants()
        {
            if (IsFull())
                return false;

            CurrentParticipants++;
            LastActivityAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// 参加者数を減少させる
        /// </summary>
        /// <returns>減少に成功した場合はtrue</returns>
        public bool DecrementParticipants()
        {
            if (CurrentParticipants <= 0)
                return false;

            CurrentParticipants--;
            LastActivityAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// ルームを削除する（論理削除）
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
            Status = RoomStatus.Closed;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// ルームを閉じる
        /// </summary>
        public void Close()
        {
            Status = RoomStatus.Closed;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// ルームを再開する
        /// </summary>
        public void Reopen()
        {
            if (!IsDeleted)
            {
                Status = RoomStatus.Active;
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
    }

    /// <summary>
    /// ルームの状態を定義する列挙型
    /// </summary>
    public enum RoomStatus
    {
        /// <summary>
        /// アクティブ（参加可能）
        /// </summary>
        Active = 0,

        /// <summary>
        /// 一時停止中
        /// </summary>
        Paused = 1,

        /// <summary>
        /// 閉鎖済み
        /// </summary>
        Closed = 2,

        /// <summary>
        /// ゲーム進行中
        /// </summary>
        InGame = 3
    }

    /// <summary>
    /// ルームのタイプを定義する列挙型
    /// </summary>
    public enum RoomType
    {
        /// <summary>
        /// 公開ルーム
        /// </summary>
        Public = 0,

        /// <summary>
        /// プライベートルーム
        /// </summary>
        Private = 1,

        /// <summary>
        /// フレンド限定ルーム
        /// </summary>
        FriendsOnly = 2,

        /// <summary>
        /// 招待制ルーム
        /// </summary>
        InviteOnly = 3
    }
}