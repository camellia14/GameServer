using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// ルーム情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomInfo
    {
        /// <summary>
        /// ルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// ルーム名
        /// </summary>
        [Key(1)]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// ルームの説明
        /// </summary>
        [Key(2)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// オーナーのユーザーID
        /// </summary>
        [Key(3)]
        public int OwnerUserId { get; set; }

        /// <summary>
        /// オーナーのユーザー名
        /// </summary>
        [Key(4)]
        public string OwnerUsername { get; set; } = string.Empty;

        /// <summary>
        /// オーナーの表示名
        /// </summary>
        [Key(5)]
        public string OwnerDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 最大参加者数
        /// </summary>
        [Key(6)]
        public int MaxParticipants { get; set; }

        /// <summary>
        /// 現在の参加者数
        /// </summary>
        [Key(7)]
        public int CurrentParticipants { get; set; }

        /// <summary>
        /// ルームの状態
        /// </summary>
        [Key(8)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// ルームのタイプ
        /// </summary>
        [Key(9)]
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// パスワード保護されているかどうか
        /// </summary>
        [Key(10)]
        public bool IsPasswordProtected { get; set; }

        /// <summary>
        /// プライベートルームかどうか
        /// </summary>
        [Key(11)]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// フレンドのみ参加可能かどうか
        /// </summary>
        [Key(12)]
        public bool IsFriendsOnly { get; set; }

        /// <summary>
        /// ルーム作成日時
        /// </summary>
        [Key(13)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後のアクティビティ日時
        /// </summary>
        [Key(14)]
        public DateTime? LastActivityAt { get; set; }
    }

    /// <summary>
    /// ルーム作成リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomCreateRequest
    {
        /// <summary>
        /// ルーム名
        /// </summary>
        [Key(0)]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// ルームの説明
        /// </summary>
        [Key(1)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 最大参加者数
        /// </summary>
        [Key(2)]
        public int MaxParticipants { get; set; } = 10;

        /// <summary>
        /// パスワード（任意）
        /// </summary>
        [Key(3)]
        public string? Password { get; set; }

        /// <summary>
        /// ルームのタイプ
        /// </summary>
        [Key(4)]
        public string RoomType { get; set; } = "Public";

        /// <summary>
        /// フレンドのみ参加可能かどうか
        /// </summary>
        [Key(5)]
        public bool IsFriendsOnly { get; set; } = false;
    }

    /// <summary>
    /// ルーム参加リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomJoinRequest
    {
        /// <summary>
        /// 参加するルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// パスワード（必要な場合）
        /// </summary>
        [Key(1)]
        public string? Password { get; set; }
    }

    /// <summary>
    /// ルーム退出リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomLeaveRequest
    {
        /// <summary>
        /// 退出するルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }
    }

    /// <summary>
    /// ルーム更新リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomUpdateRequest
    {
        /// <summary>
        /// 更新するルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// 新しいルーム名
        /// </summary>
        [Key(1)]
        public string? RoomName { get; set; }

        /// <summary>
        /// 新しい説明
        /// </summary>
        [Key(2)]
        public string? Description { get; set; }

        /// <summary>
        /// 新しい最大参加者数
        /// </summary>
        [Key(3)]
        public int? MaxParticipants { get; set; }

        /// <summary>
        /// 新しいパスワード
        /// </summary>
        [Key(4)]
        public string? Password { get; set; }
    }

    /// <summary>
    /// ルーム検索リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomSearchRequest
    {
        /// <summary>
        /// 検索キーワード
        /// </summary>
        [Key(0)]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// ルームタイプフィルター
        /// </summary>
        [Key(1)]
        public string? RoomType { get; set; }

        /// <summary>
        /// パスワード保護されていないルームのみ
        /// </summary>
        [Key(2)]
        public bool? PasswordFreeOnly { get; set; }

        /// <summary>
        /// 満員ではないルームのみ
        /// </summary>
        [Key(3)]
        public bool? NotFullOnly { get; set; } = true;

        /// <summary>
        /// 最大検索結果数
        /// </summary>
        [Key(4)]
        public int MaxResults { get; set; } = 20;

        /// <summary>
        /// 取得開始位置
        /// </summary>
        [Key(5)]
        public int Offset { get; set; } = 0;
    }

    /// <summary>
    /// ルームメンバー情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomMemberInfo
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
        /// メンバーの役割
        /// </summary>
        [Key(4)]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 参加日時
        /// </summary>
        [Key(5)]
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// 最後のアクティビティ日時
        /// </summary>
        [Key(6)]
        public DateTime? LastActivityAt { get; set; }

        /// <summary>
        /// オンライン状態
        /// </summary>
        [Key(7)]
        public bool IsOnline { get; set; }
    }

    /// <summary>
    /// ルーム招待リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomInviteRequest
    {
        /// <summary>
        /// 招待するルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// 招待するユーザーID
        /// </summary>
        [Key(1)]
        public int TargetUserId { get; set; }

        /// <summary>
        /// 招待メッセージ
        /// </summary>
        [Key(2)]
        public string? InviteMessage { get; set; }
    }

    /// <summary>
    /// ルームキック/BAN リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomModerationRequest
    {
        /// <summary>
        /// 対象ルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// 対象ユーザーID
        /// </summary>
        [Key(1)]
        public int TargetUserId { get; set; }

        /// <summary>
        /// 理由
        /// </summary>
        [Key(2)]
        public string? Reason { get; set; }
    }

    /// <summary>
    /// ルーム権限変更リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomRoleChangeRequest
    {
        /// <summary>
        /// 対象ルームID
        /// </summary>
        [Key(0)]
        public int RoomId { get; set; }

        /// <summary>
        /// 対象ユーザーID
        /// </summary>
        [Key(1)]
        public int TargetUserId { get; set; }

        /// <summary>
        /// 新しい役割
        /// </summary>
        [Key(2)]
        public string NewRole { get; set; } = string.Empty;
    }

    /// <summary>
    /// フレンドルーム検索結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class FriendRoomInfo
    {
        /// <summary>
        /// ルーム情報
        /// </summary>
        [Key(0)]
        public RoomInfo Room { get; set; } = new RoomInfo();

        /// <summary>
        /// フレンドのユーザーID
        /// </summary>
        [Key(1)]
        public int FriendUserId { get; set; }

        /// <summary>
        /// フレンドのユーザー名
        /// </summary>
        [Key(2)]
        public string FriendUsername { get; set; } = string.Empty;

        /// <summary>
        /// フレンドの表示名
        /// </summary>
        [Key(3)]
        public string FriendDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// フレンドがルームオーナーかどうか
        /// </summary>
        [Key(4)]
        public bool IsFriendOwner { get; set; }

        /// <summary>
        /// フレンドの役割
        /// </summary>
        [Key(5)]
        public string FriendRole { get; set; } = string.Empty;
    }

    /// <summary>
    /// ルーム統計情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class RoomStatistics
    {
        /// <summary>
        /// 作成したルーム数
        /// </summary>
        [Key(0)]
        public int CreatedRooms { get; set; }

        /// <summary>
        /// 参加したルーム数
        /// </summary>
        [Key(1)]
        public int JoinedRooms { get; set; }

        /// <summary>
        /// 現在参加中のルーム数
        /// </summary>
        [Key(2)]
        public int CurrentRooms { get; set; }

        /// <summary>
        /// 合計ルーム滞在時間（分）
        /// </summary>
        [Key(3)]
        public int TotalRoomTimeMinutes { get; set; }

        /// <summary>
        /// 招待したメンバー数
        /// </summary>
        [Key(4)]
        public int InvitedMembers { get; set; }

        /// <summary>
        /// 最後にルームに参加した日時
        /// </summary>
        [Key(5)]
        public DateTime? LastRoomJoinAt { get; set; }
    }
}