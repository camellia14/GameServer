using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// アカウント関連のRPCサービスインターフェース
    /// アカウントの登録、変更、管理を提供する
    /// </summary>
    public interface IAccountService : IService<IAccountService>
    {
        /// <summary>
        /// 新しいアカウントを登録する
        /// </summary>
        /// <param name="request">アカウント登録リクエスト</param>
        /// <returns>登録結果</returns>
        UnaryResult<OperationResult> RegisterAccount(AccountRegistrationRequest request);

        /// <summary>
        /// 現在のアカウント情報を取得する
        /// </summary>
        /// <returns>アカウント情報</returns>
        UnaryResult<AccountData?> GetMyAccount();

        /// <summary>
        /// アカウント情報を更新する
        /// </summary>
        /// <param name="request">アカウント更新リクエスト</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateAccount(AccountUpdateRequest request);

        /// <summary>
        /// アカウントを削除する（論理削除）
        /// </summary>
        /// <returns>削除結果</returns>
        UnaryResult<OperationResult> DeleteAccount();

        /// <summary>
        /// パスワードを変更する
        /// </summary>
        /// <param name="currentPassword">現在のパスワード</param>
        /// <param name="newPassword">新しいパスワード</param>
        /// <returns>変更結果</returns>
        UnaryResult<OperationResult> ChangePassword(string currentPassword, string newPassword);

        /// <summary>
        /// 指定されたアカウント情報を取得する（管理者機能）
        /// </summary>
        /// <param name="accountId">アカウントID</param>
        /// <returns>アカウント情報</returns>
        UnaryResult<AccountData?> GetAccount(int accountId);

        /// <summary>
        /// アカウントを停止する（管理者機能）
        /// </summary>
        /// <param name="accountId">停止するアカウントID</param>
        /// <param name="reason">停止理由</param>
        /// <returns>停止結果</returns>
        UnaryResult<OperationResult> SuspendAccount(int accountId, string reason);

        /// <summary>
        /// アカウントの停止を解除する（管理者機能）
        /// </summary>
        /// <param name="accountId">停止解除するアカウントID</param>
        /// <returns>停止解除結果</returns>
        UnaryResult<OperationResult> UnsuspendAccount(int accountId);
    }
}