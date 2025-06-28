using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    public interface IAdminService : IService<IAdminService>
    {
        UnaryResult<bool> SuspendUser(int targetUserId, int adminUserId, string reason);
        UnaryResult<bool> ReactivateUser(int targetUserId, int adminUserId);
        UnaryResult<bool> DeleteUser(int targetUserId, int adminUserId);
        UnaryResult<List<AdminData>> GetSuspendedUsers(int adminUserId);
        UnaryResult<bool> IsAdmin(int userId);
        UnaryResult<bool> PromoteToAdmin(int targetUserId, int adminUserId);
    }
}