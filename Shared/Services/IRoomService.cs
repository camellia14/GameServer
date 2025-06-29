using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// ルーム関連のRPCサービスインターフェース
    /// ルームの作成、参加、管理機能を提供する
    /// </summary>
    public interface IRoomService : IService<IRoomService>
    {
        /// <summary>
        /// ルームを作成する
        /// </summary>
        /// <param name="request">ルーム作成リクエスト</param>
        /// <returns>作成されたルーム情報</returns>
        UnaryResult<OperationResult<RoomInfo>> CreateRoom(RoomCreateRequest request);

        /// <summary>
        /// ルームに参加する
        /// </summary>
        /// <param name="request">ルーム参加リクエスト</param>
        /// <returns>参加結果</returns>
        UnaryResult<OperationResult> JoinRoom(RoomJoinRequest request);

        /// <summary>
        /// ルームから退出する
        /// </summary>
        /// <param name="request">ルーム退出リクエスト</param>
        /// <returns>退出結果</returns>
        UnaryResult<OperationResult> LeaveRoom(RoomLeaveRequest request);

        /// <summary>
        /// ルーム情報を取得する
        /// </summary>
        /// <param name="roomId">ルームID</param>
        /// <returns>ルーム情報</returns>
        UnaryResult<OperationResult<RoomInfo>> GetRoomInfo(int roomId);

        /// <summary>
        /// ルーム情報を更新する（オーナー・管理者のみ）
        /// </summary>
        /// <param name="request">ルーム更新リクエスト</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateRoom(RoomUpdateRequest request);

        /// <summary>
        /// ルームを削除する（オーナーのみ）
        /// </summary>
        /// <param name="roomId">削除するルームID</param>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> DeleteRoom(int roomId);

        /// <summary>
        /// ルームメンバー一覧を取得する
        /// </summary>
        /// <param name="roomId">ルームID</param>
        /// <returns>メンバー一覧</returns>
        UnaryResult<List<RoomMemberInfo>> GetRoomMembers(int roomId);

        /// <summary>
        /// ルームを検索する（パスワード無しの部屋限定）
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <returns>検索結果</returns>
        UnaryResult<List<RoomInfo>> SearchRooms(RoomSearchRequest request);

        /// <summary>
        /// フレンドのルームを検索する
        /// </summary>
        /// <returns>フレンドが参加中のルーム一覧</returns>
        UnaryResult<List<FriendRoomInfo>> SearchFriendRooms();

        /// <summary>
        /// ユーザーをルームに招待する
        /// </summary>
        /// <param name="request">招待リクエスト</param>
        /// <returns>招待結果</returns>
        UnaryResult<OperationResult> InviteToRoom(RoomInviteRequest request);

        /// <summary>
        /// ユーザーをルームからキックする（モデレーター以上）
        /// </summary>
        /// <param name="request">キックリクエスト</param>
        /// <returns>キック結果</returns>
        UnaryResult<OperationResult> KickUser(RoomModerationRequest request);

        /// <summary>
        /// ユーザーをルームからBANする（管理者以上）
        /// </summary>
        /// <param name="request">BANリクエスト</param>
        /// <returns>BAN結果</returns>
        UnaryResult<OperationResult> BanUser(RoomModerationRequest request);

        /// <summary>
        /// ユーザーのBAN状態を解除する（管理者以上）
        /// </summary>
        /// <param name="request">BAN解除リクエスト</param>
        /// <returns>BAN解除結果</returns>
        UnaryResult<OperationResult> UnbanUser(RoomModerationRequest request);

        /// <summary>
        /// メンバーの役割を変更する（オーナー・管理者のみ）
        /// </summary>
        /// <param name="request">役割変更リクエスト</param>
        /// <returns>変更結果</returns>
        UnaryResult<OperationResult> ChangeUserRole(RoomRoleChangeRequest request);

        /// <summary>
        /// ルームを閉じる（オーナーのみ）
        /// </summary>
        /// <param name="roomId">閉じるルームID</param>
        /// <returns>閉鎖結果</returns>
        UnaryResult<OperationResult> CloseRoom(int roomId);

        /// <summary>
        /// ルームを再開する（オーナーのみ）
        /// </summary>
        /// <param name="roomId">再開するルームID</param>
        /// <returns>再開結果</returns>
        UnaryResult<OperationResult> ReopenRoom(int roomId);

        /// <summary>
        /// ユーザーが参加中のルーム一覧を取得する
        /// </summary>
        /// <returns>参加中ルーム一覧</returns>
        UnaryResult<List<RoomInfo>> GetMyRooms();

        /// <summary>
        /// ユーザーが作成したルーム一覧を取得する
        /// </summary>
        /// <returns>作成したルーム一覧</returns>
        UnaryResult<List<RoomInfo>> GetMyCreatedRooms();

        /// <summary>
        /// ルーム統計情報を取得する
        /// </summary>
        /// <returns>統計情報</returns>
        UnaryResult<RoomStatistics> GetRoomStatistics();

        /// <summary>
        /// BANされたユーザー一覧を取得する（管理者以上）
        /// </summary>
        /// <param name="roomId">ルームID</param>
        /// <returns>BANユーザー一覧</returns>
        UnaryResult<List<RoomMemberInfo>> GetBannedUsers(int roomId);

        /// <summary>
        /// ルームのオーナーを移譲する（現在のオーナーのみ）
        /// </summary>
        /// <param name="roomId">ルームID</param>
        /// <param name="newOwnerId">新しいオーナーのユーザーID</param>
        /// <returns>移譲結果</returns>
        UnaryResult<OperationResult> TransferOwnership(int roomId, int newOwnerId);

        /// <summary>
        /// パスワードを設定/変更する（オーナー・管理者のみ）
        /// </summary>
        /// <param name="roomId">ルームID</param>
        /// <param name="newPassword">新しいパスワード（nullで削除）</param>
        /// <returns>設定結果</returns>
        UnaryResult<OperationResult> SetRoomPassword(int roomId, string? newPassword);

        /// <summary>
        /// 人気のルーム一覧を取得する
        /// </summary>
        /// <param name="maxResults">最大取得数</param>
        /// <returns>人気ルーム一覧</returns>
        UnaryResult<List<RoomInfo>> GetPopularRooms(int maxResults = 10);

        /// <summary>
        /// 最近作成されたルーム一覧を取得する
        /// </summary>
        /// <param name="maxResults">最大取得数</param>
        /// <returns>最近作成されたルーム一覧</returns>
        UnaryResult<List<RoomInfo>> GetRecentRooms(int maxResults = 10);

        /// <summary>
        /// ルームの履歴を削除する（管理者機能）
        /// </summary>
        /// <param name="olderThanDays">指定日数より古い履歴を削除</param>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> CleanupRoomHistory(int olderThanDays);
    }
}