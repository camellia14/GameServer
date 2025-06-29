using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// チャットエンティティクラス
    /// チャットメッセージの情報を管理する
    /// </summary>
    [Table("chats")]
    public class ChatEntity
    {
        /// <summary>
        /// チャットメッセージの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }

        /// <summary>
        /// メッセージを送信したプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int SenderUserId { get; set; }

        /// <summary>
        /// チャットの種類
        /// </summary>
        [Required]
        public ChatType ChatType { get; set; }

        /// <summary>
        /// チャットメッセージの内容
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 受信者のユーザーID（個人チャットの場合）
        /// </summary>
        public int? ReceiverUserId { get; set; }

        /// <summary>
        /// グループID（グループチャットの場合）
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// ルームID（ルームチャットの場合）
        /// </summary>
        public int? RoomId { get; set; }

        /// <summary>
        /// メッセージの送信日時
        /// </summary>
        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// メッセージが既読かどうか
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// メッセージの既読日時
        /// </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>
        /// メッセージが編集されたかどうか
        /// </summary>
        public bool IsEdited { get; set; } = false;

        /// <summary>
        /// メッセージの編集日時
        /// </summary>
        public DateTime? EditedAt { get; set; }

        /// <summary>
        /// メッセージが削除されたかどうか
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// メッセージの削除日時
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// システムメッセージかどうか
        /// </summary>
        public bool IsSystemMessage { get; set; } = false;

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
        /// 送信者プレイヤーエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(SenderUserId))]
        public virtual PlayerEntity? SenderPlayer { get; set; }

        /// <summary>
        /// 受信者プレイヤーエンティティとの関連（個人チャットの場合）
        /// </summary>
        [ForeignKey(nameof(ReceiverUserId))]
        public virtual PlayerEntity? ReceiverPlayer { get; set; }

        /// <summary>
        /// メッセージを既読にする
        /// </summary>
        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メッセージを編集する
        /// </summary>
        /// <param name="newMessage">新しいメッセージ内容</param>
        public void Edit(string newMessage)
        {
            if (!IsDeleted && !string.IsNullOrWhiteSpace(newMessage))
            {
                Message = newMessage.Trim();
                IsEdited = true;
                EditedAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メッセージを削除する（論理削除）
        /// </summary>
        public void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                DeletedAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// メッセージが指定されたユーザーに関連するかを判定する
        /// </summary>
        /// <param name="userId">判定するユーザーID</param>
        /// <returns>関連する場合はtrue</returns>
        public bool IsRelevantToUser(int userId)
        {
            return ChatType switch
            {
                ChatType.Private => SenderUserId == userId || ReceiverUserId == userId,
                ChatType.Global => true,
                ChatType.Group => SenderUserId == userId, // グループメンバーシップは別途チェック必要
                ChatType.Room => SenderUserId == userId,  // ルームメンバーシップは別途チェック必要
                _ => false
            };
        }

        /// <summary>
        /// メッセージが表示可能かを判定する
        /// </summary>
        /// <returns>表示可能な場合はtrue</returns>
        public bool IsDisplayable()
        {
            return !IsDeleted;
        }
    }

    /// <summary>
    /// チャットの種類を定義する列挙型
    /// </summary>
    public enum ChatType
    {
        /// <summary>
        /// 個人チャット（1対1）
        /// </summary>
        Private = 0,

        /// <summary>
        /// グループチャット
        /// </summary>
        Group = 1,

        /// <summary>
        /// ルームチャット
        /// </summary>
        Room = 2,

        /// <summary>
        /// グローバルチャット（全体）
        /// </summary>
        Global = 3
    }
}