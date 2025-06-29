using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// アカウント情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class AccountData
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        [Key(0)]
        public int AccountId { get; set; }

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
        /// メールアドレス
        /// </summary>
        [Key(3)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// アカウント状態
        /// </summary>
        [Key(4)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 最終ログイン日時
        /// </summary>
        [Key(5)]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Key(6)]
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// プロフィール情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class ProfileData
    {
        /// <summary>
        /// プロフィールID
        /// </summary>
        [Key(0)]
        public int ProfileId { get; set; }

        /// <summary>
        /// アカウントID
        /// </summary>
        [Key(1)]
        public int AccountId { get; set; }

        /// <summary>
        /// プロフィール画像URL
        /// </summary>
        [Key(2)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 自己紹介文
        /// </summary>
        [Key(3)]
        public string Biography { get; set; } = string.Empty;

        /// <summary>
        /// 年齢
        /// </summary>
        [Key(4)]
        public int? Age { get; set; }

        /// <summary>
        /// 居住国
        /// </summary>
        [Key(5)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 好きなゲームジャンル
        /// </summary>
        [Key(6)]
        public string FavoriteGenre { get; set; } = string.Empty;

        /// <summary>
        /// プレイ時間（時間）
        /// </summary>
        [Key(7)]
        public double PlayTimeHours { get; set; }

        /// <summary>
        /// プレイヤーレベル
        /// </summary>
        [Key(8)]
        public int PlayerLevel { get; set; }

        /// <summary>
        /// 経験値
        /// </summary>
        [Key(9)]
        public long Experience { get; set; }

        /// <summary>
        /// 獲得実績数
        /// </summary>
        [Key(10)]
        public int AchievementCount { get; set; }

        /// <summary>
        /// プロフィール公開設定
        /// </summary>
        [Key(11)]
        public string Privacy { get; set; } = string.Empty;

        /// <summary>
        /// オンライン状態表示設定
        /// </summary>
        [Key(12)]
        public bool ShowOnlineStatus { get; set; }

        /// <summary>
        /// フレンド申請受付設定
        /// </summary>
        [Key(13)]
        public bool AcceptFriendRequests { get; set; }
    }

    /// <summary>
    /// アカウント登録リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class AccountRegistrationRequest
    {
        /// <summary>
        /// ユーザー名
        /// </summary>
        [Key(0)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Key(1)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Key(2)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 表示名
        /// </summary>
        [Key(3)]
        public string DisplayName { get; set; } = string.Empty;
    }

    /// <summary>
    /// アカウント更新リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class AccountUpdateRequest
    {
        /// <summary>
        /// 表示名
        /// </summary>
        [Key(0)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Key(1)]
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// プロフィール更新リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class ProfileUpdateRequest
    {
        /// <summary>
        /// プロフィール画像URL
        /// </summary>
        [Key(0)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 自己紹介文
        /// </summary>
        [Key(1)]
        public string Biography { get; set; } = string.Empty;

        /// <summary>
        /// 生年月日
        /// </summary>
        [Key(2)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 居住国
        /// </summary>
        [Key(3)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 好きなゲームジャンル
        /// </summary>
        [Key(4)]
        public string FavoriteGenre { get; set; } = string.Empty;

        /// <summary>
        /// プロフィール公開設定
        /// </summary>
        [Key(5)]
        public string Privacy { get; set; } = string.Empty;

        /// <summary>
        /// オンライン状態表示設定
        /// </summary>
        [Key(6)]
        public bool ShowOnlineStatus { get; set; }

        /// <summary>
        /// フレンド申請受付設定
        /// </summary>
        [Key(7)]
        public bool AcceptFriendRequests { get; set; }
    }

    /// <summary>
    /// 操作結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class OperationResult
    {
        /// <summary>
        /// 操作が成功したかどうか
        /// </summary>
        [Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// エラーコード（エラーの場合）
        /// </summary>
        [Key(2)]
        public string? ErrorCode { get; set; }
    }
}