using MagicOnion;
using MagicOnion.Server;
using Shared.Services;
using Shared.Data;
using GameServer.UseCases;

namespace GameServer.Services
{
    public class AdminService : ServiceBase<IAdminService>, IAdminService
    {
        private readonly AdminUseCase _adminUseCase;

        public AdminService(AdminUseCase adminUseCase)
        {
            _adminUseCase = adminUseCase;
        }

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