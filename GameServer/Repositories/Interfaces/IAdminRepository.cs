using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<UserRoleEntity?> GetUserRoleAsync(int userId);
        Task<UserRoleEntity> CreateUserRoleAsync(UserRoleEntity userRole);
        Task<UserRoleEntity> UpdateUserRoleAsync(UserRoleEntity userRole);
        Task<bool> IsAdminAsync(int userId);
        Task<List<UserRoleEntity>> GetSuspendedUsersAsync();
        Task SuspendUserAsync(int targetUserId, int adminUserId, string reason);
        Task ReactivateUserAsync(int targetUserId, int adminUserId);
        Task DeleteUserAsync(int targetUserId, int adminUserId);
    }
}