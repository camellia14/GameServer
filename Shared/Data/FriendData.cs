using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// フレンド情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendData
    {
        /// <summary>
        /// フレンドID
        /// </summary>
        [Key(0)]
        public int FriendId { get; set; }

        /// <summary>
        /// フレンドのユーザーID
        /// </summary>
        [Key(1)]
        public int UserId { get; set; }

        /// <summary>
        /// フレンドのユーザー名
        /// </summary>
        [Key(2)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// フレンドの表示名
        /// </summary>
        [Key(3)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// フレンドのアバターURL
        /// </summary>
        [Key(4)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// フレンドのプレイヤーレベル
        /// </summary>
        [Key(5)]
        public int PlayerLevel { get; set; }

        /// <summary>
        /// フレンド関係の状態
        /// </summary>
        [Key(6)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// オンライン状態
        /// </summary>
        [Key(7)]
        public bool IsOnline { get; set; }

        /// <summary>
        /// 最終ログイン日時
        /// </summary>
        [Key(8)]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// フレンド申請日時
        /// </summary>
        [Key(9)]
        public DateTime RequestedAt { get; set; }

        /// <summary>
        /// 申請メッセージ
        /// </summary>
        [Key(10)]
        public string RequestMessage { get; set; } = string.Empty;

        /// <summary>
        /// お気に入り設定
        /// </summary>
        [Key(11)]
        public bool IsFavorite { get; set; }

        /// <summary>
        /// ブロック状態
        /// </summary>
        [Key(12)]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// 最後に一緒にプレイした日時
        /// </summary>
        [Key(13)]
        public DateTime? LastPlayedTogetherAt { get; set; }

        /// <summary>
        /// 申請者かどうか
        /// </summary>
        [Key(14)]
        public bool IsRequester { get; set; }
    }

    /// <summary>
    /// フレンド申請リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendRequestData
    {
        /// <summary>
        /// 申請対象のユーザーID
        /// </summary>
        [Key(0)]
        public int TargetUserId { get; set; }

        /// <summary>
        /// 申請メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// フレンド申請応答リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendResponseData
    {
        /// <summary>
        /// フレンドID
        /// </summary>
        [Key(0)]
        public int FriendId { get; set; }

        /// <summary>
        /// 承認するかどうか（true: 承認, false: 拒否）
        /// </summary>
        [Key(1)]
        public bool Accept { get; set; }
    }

    /// <summary>
    /// フレンド検索リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendSearchRequest
    {
        /// <summary>
        /// 検索キーワード（ユーザー名または表示名）
        /// </summary>
        [Key(0)]
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// 最大検索結果数
        /// </summary>
        [Key(1)]
        public int MaxResults { get; set; } = 20;
    }

    /// <summary>
    /// フレンド検索結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendSearchResult
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Key(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [Key(1)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 表示名
        /// </summary>
        [Key(2)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// アバターURL
        /// </summary>
        [Key(3)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// プレイヤーレベル
        /// </summary>
        [Key(4)]
        public int PlayerLevel { get; set; }

        /// <summary>
        /// 既にフレンドかどうか
        /// </summary>
        [Key(5)]
        public bool IsAlreadyFriend { get; set; }

        /// <summary>
        /// 申請中かどうか
        /// </summary>
        [Key(6)]
        public bool IsPendingRequest { get; set; }

        /// <summary>
        /// オンライン状態
        /// </summary>
        [Key(7)]
        public bool IsOnline { get; set; }
    }

    /// <summary>
    /// フレンド統計情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendStatistics
    {
        /// <summary>
        /// 総フレンド数
        /// </summary>
        [Key(0)]
        public int TotalFriends { get; set; }

        /// <summary>
        /// オンラインフレンド数
        /// </summary>
        [Key(1)]
        public int OnlineFriends { get; set; }

        /// <summary>
        /// 受信した申請数
        /// </summary>
        [Key(2)]
        public int IncomingRequests { get; set; }

        /// <summary>
        /// 送信した申請数
        /// </summary>
        [Key(3)]
        public int OutgoingRequests { get; set; }

        /// <summary>
        /// ブロック中のユーザー数
        /// </summary>
        [Key(4)]
        public int BlockedUsers { get; set; }

        /// <summary>
        /// お気に入りフレンド数
        /// </summary>
        [Key(5)]
        public int FavoriteFriends { get; set; }
    }
}