using MagicOnion;
using MagicOnion.Server;
using Shared.Services;
using Shared.Data;
using GameServer.UseCases;

namespace GameServer.Services
{
    /// <summary>
    /// 管理者機能のRPCサービスを提供するクラス
    /// ユーザーの権限管理、アカウント停止・復旧・削除機能を担当する
    /// </summary>
    public class AdminService : ServiceBase<IAdminService>, IAdminService
    {
        private readonly AdminUseCase _adminUseCase;

        /// <summary>
        /// AdminServiceのコンストラクタ
        /// </summary>
        /// <param name="adminUseCase">管理者ビジネスロジック</param>
        public AdminService(AdminUseCase adminUseCase)
        {
            _adminUseCase = adminUseCase;
        }

        /// <summary>
        /// 指定されたユーザーアカウントを停止する
        /// </summary>
        /// <param name="targetUserId">停止対象のユーザーID</param>
        /// <param name="adminUserId">停止処理を実行する管理者のユーザーID</param>
        /// <param name="reason">停止理由</param>
        /// <returns>停止処理に成功した場合はtrue、失敗した場合はfalse</returns>
        public async UnaryResult<bool> SuspendUser(int targetUserId, int adminUserId, string reason)
        {
            try
            {
                await _adminUseCase.SuspendUserAsync(targetUserId, adminUserId, reason);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SuspendUser: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止されたユーザーアカウントを復旧する
        /// </summary>
        /// <param name="targetUserId">復旧対象のユーザーID</param>
        /// <param name="adminUserId">復旧処理を実行する管理者のユーザーID</param>
        /// <returns>復旧処理に成功した場合はtrue、失敗した場合はfalse</returns>
        public async UnaryResult<bool> ReactivateUser(int targetUserId, int adminUserId)
        {
            try
            {
                await _adminUseCase.ReactivateUserAsync(targetUserId, adminUserId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReactivateUser: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 指定されたユーザーアカウントを削除する
        /// </summary>
        /// <param name="targetUserId">削除対象のユーザーID</param>
        /// <param name="adminUserId">削除処理を実行する管理者のユーザーID</param>
        /// <returns>削除処理に成功した場合はtrue、失敗した場合はfalse</returns>
        public async UnaryResult<bool> DeleteUser(int targetUserId, int adminUserId)
        {
            try
            {
                await _adminUseCase.DeleteUserAsync(targetUserId, adminUserId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUser: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止状態のユーザー一覧を取得する
        /// </summary>
        /// <param name="adminUserId">情報を要求する管理者のユーザーID</param>
        /// <returns>停止中のユーザー情報リスト</returns>
        public async UnaryResult<List<AdminData>> GetSuspendedUsers(int adminUserId)
        {
            try
            {
                return await _adminUseCase.GetSuspendedUsersAsync(adminUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetSuspendedUsers: {ex.Message}");
                return new List<AdminData>();
            }
        }

        /// <summary>
        /// 指定されたユーザーが管理者権限を持っているかを確認する
        /// </summary>
        /// <param name="userId">確認対象のユーザーID</param>
        /// <returns>管理者権限を持っている場合はtrue、そうでなければfalse</returns>
        public async UnaryResult<bool> IsAdmin(int userId)
        {
            try
            {
                return await _adminUseCase.IsAdminAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsAdmin: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 指定されたユーザーを管理者に昇格させる
        /// </summary>
        /// <param name="targetUserId">昇格対象のユーザーID</param>
        /// <param name="adminUserId">昇格処理を実行する管理者のユーザーID</param>
        /// <returns>昇格処理に成功した場合はtrue、失敗した場合はfalse</returns>
        public async UnaryResult<bool> PromoteToAdmin(int targetUserId, int adminUserId)
        {
            try
            {
                await _adminUseCase.PromoteToAdminAsync(targetUserId, adminUserId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PromoteToAdmin: {ex.Message}");
                return false;
            }
        }
    }
}