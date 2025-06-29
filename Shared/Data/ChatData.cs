using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// チャットメッセージのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatMessage
    {
        /// <summary>
        /// チャットID
        /// </summary>
        [Key(0)]
        public int ChatId { get; set; }

        /// <summary>
        /// 送信者のユーザーID
        /// </summary>
        [Key(1)]
        public int SenderUserId { get; set; }

        /// <summary>
        /// 送信者のユーザー名
        /// </summary>
        [Key(2)]
        public string SenderUsername { get; set; } = string.Empty;

        /// <summary>
        /// 送信者の表示名
        /// </summary>
        [Key(3)]
        public string SenderDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// チャットの種類
        /// </summary>
        [Key(4)]
        public string ChatType { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ内容
        /// </summary>
        [Key(5)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 受信者のユーザーID（個人チャットの場合）
        /// </summary>
        [Key(6)]
        public int? ReceiverUserId { get; set; }

        /// <summary>
        /// グループID（グループチャットの場合）
        /// </summary>
        [Key(7)]
        public int? GroupId { get; set; }

        /// <summary>
        /// ルームID（ルームチャットの場合）
        /// </summary>
        [Key(8)]
        public int? RoomId { get; set; }

        /// <summary>
        /// 送信日時
        /// </summary>
        [Key(9)]
        public DateTime SentAt { get; set; }

        /// <summary>
        /// 既読かどうか
        /// </summary>
        [Key(10)]
        public bool IsRead { get; set; }

        /// <summary>
        /// 編集されたかどうか
        /// </summary>
        [Key(11)]
        public bool IsEdited { get; set; }

        /// <summary>
        /// システムメッセージかどうか
        /// </summary>
        [Key(12)]
        public bool IsSystemMessage { get; set; }

        /// <summary>
        /// 送信者のアバターURL
        /// </summary>
        [Key(13)]
        public string SenderAvatarUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// チャット送信リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatSendRequest
    {
        /// <summary>
        /// チャットの種類
        /// </summary>
        [Key(0)]
        public string ChatType { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ内容
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 受信者のユーザーID（個人チャットの場合）
        /// </summary>
        [Key(2)]
        public int? ReceiverUserId { get; set; }

        /// <summary>
        /// グループID（グループチャットの場合）
        /// </summary>
        [Key(3)]
        public int? GroupId { get; set; }

        /// <summary>
        /// ルームID（ルームチャットの場合）
        /// </summary>
        [Key(4)]
        public int? RoomId { get; set; }
    }

    /// <summary>
    /// チャット履歴取得リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatHistoryRequest
    {
        /// <summary>
        /// チャットの種類
        /// </summary>
        [Key(0)]
        public string ChatType { get; set; } = string.Empty;

        /// <summary>
        /// 相手のユーザーID（個人チャットの場合）
        /// </summary>
        [Key(1)]
        public int? TargetUserId { get; set; }

        /// <summary>
        /// グループID（グループチャットの場合）
        /// </summary>
        [Key(2)]
        public int? GroupId { get; set; }

        /// <summary>
        /// ルームID（ルームチャットの場合）
        /// </summary>
        [Key(3)]
        public int? RoomId { get; set; }

        /// <summary>
        /// 取得する最大メッセージ数
        /// </summary>
        [Key(4)]
        public int MaxMessages { get; set; } = 50;

        /// <summary>
        /// 取得開始位置（最新からの番号）
        /// </summary>
        [Key(5)]
        public int Offset { get; set; } = 0;
    }

    /// <summary>
    /// チャット編集リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatEditRequest
    {
        /// <summary>
        /// 編集するチャットID
        /// </summary>
        [Key(0)]
        public int ChatId { get; set; }

        /// <summary>
        /// 新しいメッセージ内容
        /// </summary>
        [Key(1)]
        public string NewMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// チャット削除リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatDeleteRequest
    {
        /// <summary>
        /// 削除するチャットID
        /// </summary>
        [Key(0)]
        public int ChatId { get; set; }
    }

    /// <summary>
    /// チャット既読マークリクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatMarkReadRequest
    {
        /// <summary>
        /// 既読にするチャットID
        /// </summary>
        [Key(0)]
        public int ChatId { get; set; }
    }

    /// <summary>
    /// チャット統計情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatStatistics
    {
        /// <summary>
        /// 送信したメッセージ数
        /// </summary>
        [Key(0)]
        public int SentMessages { get; set; }

        /// <summary>
        /// 受信したメッセージ数
        /// </summary>
        [Key(1)]
        public int ReceivedMessages { get; set; }

        /// <summary>
        /// 未読メッセージ数
        /// </summary>
        [Key(2)]
        public int UnreadMessages { get; set; }

        /// <summary>
        /// アクティブな個人チャット数
        /// </summary>
        [Key(3)]
        public int ActivePrivateChats { get; set; }

        /// <summary>
        /// 参加中のグループ数
        /// </summary>
        [Key(4)]
        public int JoinedGroups { get; set; }

        /// <summary>
        /// 最後にチャットした日時
        /// </summary>
        [Key(5)]
        public DateTime? LastChatAt { get; set; }
    }

    /// <summary>
    /// チャット会話リストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ChatConversation
    {
        /// <summary>
        /// 会話の種類
        /// </summary>
        [Key(0)]
        public string ChatType { get; set; } = string.Empty;

        /// <summary>
        /// 相手のユーザーID（個人チャットの場合）
        /// </summary>
        [Key(1)]
        public int? TargetUserId { get; set; }

        /// <summary>
        /// 相手のユーザー名
        /// </summary>
        [Key(2)]
        public string TargetUsername { get; set; } = string.Empty;

        /// <summary>
        /// 相手の表示名
        /// </summary>
        [Key(3)]
        public string TargetDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 相手のアバターURL
        /// </summary>
        [Key(4)]
        public string TargetAvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// グループ名（グループチャットの場合）
        /// </summary>
        [Key(5)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// ルーム名（ルームチャットの場合）
        /// </summary>
        [Key(6)]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// 最後のメッセージ
        /// </summary>
        [Key(7)]
        public string LastMessage { get; set; } = string.Empty;

        /// <summary>
        /// 最後のメッセージ送信日時
        /// </summary>
        [Key(8)]
        public DateTime LastMessageAt { get; set; }

        /// <summary>
        /// 未読メッセージ数
        /// </summary>
        [Key(9)]
        public int UnreadCount { get; set; }

        /// <summary>
        /// オンライン状態（個人チャットの場合）
        /// </summary>
        [Key(10)]
        public bool IsOnline { get; set; }
    }
}