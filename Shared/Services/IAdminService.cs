using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// 管理者機能のRPCサービスインターフェース
    /// クライアント・サーバー間の管理者操作API定義
    /// </summary>
    public interface IAdminService : IService<IAdminService>
    {
        /// <summary>
        /// 指定されたユーザーアカウントを停止する
        /// </summary>
        /// <param name="targetUserId">停止対象のユーザーID</param>
        /// <param name="adminUserId">停止処理を実行する管理者のユーザーID</param>
        /// <param name="reason">停止理由</param>
        /// <returns>停止処理に成功した場合はtrue、失敗した場合はfalse</returns>
        UnaryResult<bool> SuspendUser(int targetUserId, int adminUserId, string reason);

        /// <summary>
        /// 停止されたユーザーアカウントを復旧する
        /// </summary>
        /// <param name="targetUserId">復旧対象のユーザーID</param>
        /// <param name="adminUserId">復旧処理を実行する管理者のユーザーID</param>
        /// <returns>復旧処理に成功した場合はtrue、失敗した場合はfalse</returns>
        UnaryResult<bool> ReactivateUser(int targetUserId, int adminUserId);

        /// <summary>
        /// 指定されたユーザーアカウントを削除する
        /// </summary>
        /// <param name="targetUserId">削除対象のユーザーID</param>
        /// <param name="adminUserId">削除処理を実行する管理者のユーザーID</param>
        /// <returns>削除処理に成功した場合はtrue、失敗した場合はfalse</returns>
        UnaryResult<bool> DeleteUser(int targetUserId, int adminUserId);

        /// <summary>
        /// 停止状態のユーザー一覧を取得する
        /// </summary>
        /// <param name="adminUserId">情報を要求する管理者のユーザーID</param>
        /// <returns>停止中のユーザー情報リスト</returns>
        UnaryResult<List<AdminData>> GetSuspendedUsers(int adminUserId);

        /// <summary>
        /// 指定されたユーザーが管理者権限を持っているかを確認する
        /// </summary>
        /// <param name="userId">確認対象のユーザーID</param>
        /// <returns>管理者権限を持っている場合はtrue、そうでなければfalse</returns>
        UnaryResult<bool> IsAdmin(int userId);

        /// <summary>
        /// 指定されたユーザーを管理者に昇格させる
        /// </summary>
        /// <param name="targetUserId">昇格対象のユーザーID</param>
        /// <param name="adminUserId">昇格処理を実行する管理者のユーザーID</param>
        /// <returns>昇格処理に成功した場合はtrue、失敗した場合はfalse</returns>
        UnaryResult<bool> PromoteToAdmin(int targetUserId, int adminUserId);
    }
}