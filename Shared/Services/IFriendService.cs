using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// フレンド関連のRPCサービスインターフェース
    /// フレンド申請、承認、管理機能を提供する
    /// </summary>
    public interface IFriendService : IService<IFriendService>
    {
        /// <summary>
        /// 自分のフレンド一覧を取得する
        /// </summary>
        /// <returns>フレンドリスト</returns>
        UnaryResult<List<FriendData>> GetMyFriends();

        /// <summary>
        /// 受信したフレンド申請一覧を取得する
        /// </summary>
        /// <returns>受信申請リスト</returns>
        UnaryResult<List<FriendData>> GetIncomingRequests();

        /// <summary>
        /// 送信したフレンド申請一覧を取得する
        /// </summary>
        /// <returns>送信申請リスト</returns>
        UnaryResult<List<FriendData>> GetOutgoingRequests();

        /// <summary>
        /// フレンド申請を送信する
        /// </summary>
        /// <param name="request">フレンド申請リクエスト</param>
        /// <returns>申請結果</returns>
        UnaryResult<OperationResult> SendFriendRequest(FriendRequestData request);

        /// <summary>
        /// フレンド申請に応答する（承認・拒否）
        /// </summary>
        /// <param name="response">フレンド申請応答</param>
        /// <returns>応答結果</returns>
        UnaryResult<OperationResult> RespondToFriendRequest(FriendResponseData response);

        /// <summary>
        /// フレンドを削除する
        /// </summary>
        /// <param name="friendId">削除するフレンドID</param>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> RemoveFriend(int friendId);

        /// <summary>
        /// フレンドをブロックする
        /// </summary>
        /// <param name="friendId">ブロックするフレンドID</param>
        /// <returns>ブロック結果</returns>
        UnaryResult<OperationResult> BlockFriend(int friendId);

        /// <summary>
        /// フレンドのブロックを解除する
        /// </summary>
        /// <param name="friendId">ブロック解除するフレンドID</param>
        /// <returns>ブロック解除結果</returns>
        UnaryResult<OperationResult> UnblockFriend(int friendId);

        /// <summary>
        /// フレンドのお気に入り設定を切り替える
        /// </summary>
        /// <param name="friendId">対象フレンドID</param>
        /// <returns>設定結果</returns>
        UnaryResult<OperationResult> ToggleFavoriteFriend(int friendId);

        /// <summary>
        /// オンラインフレンド一覧を取得する
        /// </summary>
        /// <returns>オンラインフレンドリスト</returns>
        UnaryResult<List<FriendData>> GetOnlineFriends();

        /// <summary>
        /// お気に入りフレンド一覧を取得する
        /// </summary>
        /// <returns>お気に入りフレンドリスト</returns>
        UnaryResult<List<FriendData>> GetFavoriteFriends();

        /// <summary>
        /// ブロック中のユーザー一覧を取得する
        /// </summary>
        /// <returns>ブロック中ユーザーリスト</returns>
        UnaryResult<List<FriendData>> GetBlockedUsers();

        /// <summary>
        /// フレンド検索を行う
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <returns>検索結果</returns>
        UnaryResult<List<FriendSearchResult>> SearchUsers(FriendSearchRequest request);

        /// <summary>
        /// フレンド統計情報を取得する
        /// </summary>
        /// <returns>統計情報</returns>
        UnaryResult<FriendStatistics> GetFriendStatistics();

        /// <summary>
        /// フレンドの詳細情報を取得する
        /// </summary>
        /// <param name="friendUserId">フレンドのユーザーID</param>
        /// <returns>フレンド詳細情報</returns>
        UnaryResult<FriendData?> GetFriendDetails(int friendUserId);

        /// <summary>
        /// 一緒にプレイした記録を更新する
        /// </summary>
        /// <param name="friendUserId">一緒にプレイしたフレンドのユーザーID</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdatePlayedTogether(int friendUserId);

        /// <summary>
        /// フレンド申請をキャンセルする
        /// </summary>
        /// <param name="friendId">キャンセルする申請のフレンドID</param>
        /// <returns>キャンセル結果</returns>
        UnaryResult<OperationResult> CancelFriendRequest(int friendId);
    }
}