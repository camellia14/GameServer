using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// チャット関連のRPCサービスインターフェース
    /// チャットの送信、受信、履歴管理を提供する
    /// </summary>
    public interface IChatService : IService<IChatService>
    {
        /// <summary>
        /// チャットメッセージを送信する
        /// </summary>
        /// <param name="request">チャット送信リクエスト</param>
        /// <returns>送信結果</returns>
        UnaryResult<OperationResult> SendMessage(ChatSendRequest request);

        /// <summary>
        /// チャット履歴を取得する
        /// </summary>
        /// <param name="request">履歴取得リクエスト</param>
        /// <returns>チャット履歴</returns>
        UnaryResult<List<ChatMessage>> GetChatHistory(ChatHistoryRequest request);

        /// <summary>
        /// 会話リストを取得する
        /// </summary>
        /// <returns>会話リスト</returns>
        UnaryResult<List<ChatConversation>> GetConversations();

        /// <summary>
        /// チャットメッセージを編集する
        /// </summary>
        /// <param name="request">編集リクエスト</param>
        /// <returns>編集結果</returns>
        UnaryResult<OperationResult> EditMessage(ChatEditRequest request);

        /// <summary>
        /// チャットメッセージを削除する
        /// </summary>
        /// <param name="request">削除リクエスト</param>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> DeleteMessage(ChatDeleteRequest request);

        /// <summary>
        /// チャットメッセージを既読にする
        /// </summary>
        /// <param name="request">既読マークリクエスト</param>
        /// <returns>既読マーク結果</returns>
        UnaryResult<OperationResult> MarkMessageAsRead(ChatMarkReadRequest request);

        /// <summary>
        /// 個人チャットの全メッセージを既読にする
        /// </summary>
        /// <param name="targetUserId">相手のユーザーID</param>
        /// <returns>既読マーク結果</returns>
        UnaryResult<OperationResult> MarkPrivateChatAsRead(int targetUserId);

        /// <summary>
        /// グローバルチャットの最新メッセージを取得する
        /// </summary>
        /// <param name="maxMessages">取得する最大メッセージ数</param>
        /// <returns>グローバルチャットメッセージ</returns>
        UnaryResult<List<ChatMessage>> GetGlobalChatMessages(int maxMessages = 50);

        /// <summary>
        /// 未読メッセージ数を取得する
        /// </summary>
        /// <returns>未読メッセージ数</returns>
        UnaryResult<int> GetUnreadMessageCount();

        /// <summary>
        /// 特定の相手との未読メッセージ数を取得する
        /// </summary>
        /// <param name="targetUserId">相手のユーザーID</param>
        /// <returns>未読メッセージ数</returns>
        UnaryResult<int> GetUnreadMessageCountWithUser(int targetUserId);

        /// <summary>
        /// チャット統計情報を取得する
        /// </summary>
        /// <returns>統計情報</returns>
        UnaryResult<ChatStatistics> GetChatStatistics();

        /// <summary>
        /// メッセージを検索する
        /// </summary>
        /// <param name="searchTerm">検索キーワード</param>
        /// <param name="chatType">検索対象のチャット種類（空の場合は全種類）</param>
        /// <param name="maxResults">最大検索結果数</param>
        /// <returns>検索結果</returns>
        UnaryResult<List<ChatMessage>> SearchMessages(string searchTerm, string chatType = "", int maxResults = 20);

        /// <summary>
        /// ユーザーをチャットでブロックする
        /// </summary>
        /// <param name="targetUserId">ブロックするユーザーID</param>
        /// <returns>ブロック結果</returns>
        UnaryResult<OperationResult> BlockUser(int targetUserId);

        /// <summary>
        /// ユーザーのチャットブロックを解除する
        /// </summary>
        /// <param name="targetUserId">ブロック解除するユーザーID</param>
        /// <returns>ブロック解除結果</returns>
        UnaryResult<OperationResult> UnblockUser(int targetUserId);

        /// <summary>
        /// ブロック中のユーザー一覧を取得する
        /// </summary>
        /// <returns>ブロック中ユーザーリスト</returns>
        UnaryResult<List<int>> GetBlockedUsers();

        /// <summary>
        /// チャット履歴を削除する（管理者機能）
        /// </summary>
        /// <param name="olderThanDays">指定日数より古い履歴を削除</param>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> CleanupChatHistory(int olderThanDays);
    }
}