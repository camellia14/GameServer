using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Shared.Data;

namespace GameServer.UseCases
{
    public class AdminUseCase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPlayerRepository _playerRepository;

        public AdminUseCase(IAdminRepository adminRepository, IPlayerRepository playerRepository)
        {
            _adminRepository = adminRepository;
            _playerRepository = playerRepository;
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            return await _adminRepository.IsAdminAsync(userId);
        }

        public async Task SuspendUserAsync(int targetUserId, int adminUserId, string reason)
        {
            if (!await _adminRepository.IsAdminAsync(adminUserId))
                throw new UnauthorizedAccessException("Only admins can suspend users");

            if (targetUserId == adminUserId)
                throw new InvalidOperationException("Admins cannot suspend themselves");

            var targetUser = await _playerRepository.GetPlayerAsync(targetUserId);
            if (targetUser == null)
                throw new InvalidOperationException("Target user not found");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Suspension reason is required");

            await _adminRepository.SuspendUserAsync(targetUserId, adminUserId, reason.Trim());
        }

        public async Task ReactivateUserAsync(int targetUserId, int adminUserId)
        {
            if (!await _adminRepository.IsAdminAsync(adminUserId))
                throw new UnauthorizedAccessException("Only admins can reactivate users");

            var targetUser = await _playerRepository.GetPlayerAsync(targetUserId);
            if (targetUser == null)
                throw new InvalidOperationException("Target user not found");

            await _adminRepository.ReactivateUserAsync(targetUserId, adminUserId);
        }

        public async Task DeleteUserAsync(int targetUserId, int adminUserId)
        {
            if (!await _adminRepository.IsAdminAsync(adminUserId))
                throw new UnauthorizedAccessException("Only admins can delete users");

            if (targetUserId == adminUserId)
                throw new InvalidOperationException("Admins cannot delete themselves");

            var targetUser = await _playerRepository.GetPlayerAsync(targetUserId);
            if (targetUser == null)
                throw new InvalidOperationException("Target user not found");

            await _adminRepository.DeleteUserAsync(targetUserId, adminUserId);
        }

        public async Task<List<AdminData>> GetSuspendedUsersAsync(int adminUserId)
        {
            if (!await _adminRepository.IsAdminAsync(adminUserId))
                throw new UnauthorizedAccessException("Only admins can view suspended users");

            var suspendedUsers = await _adminRepository.GetSuspendedUsersAsync();
            return suspendedUsers.Select(MapToAdminData).ToList();
        }

        public async Task PromoteToAdminAsync(int targetUserId, int adminUserId)
        {
            if (!await _adminRepository.IsAdminAsync(adminUserId))
                throw new UnauthorizedAccessException("Only admins can promote users");

            var targetUser = await _playerRepository.GetPlayerAsync(targetUserId);
            if (targetUser == null)
                throw new InvalidOperationException("Target user not found");

            var userRole = await _adminRepository.GetUserRoleAsync(targetUserId);
            if (userRole == null)
            {
                userRole = new UserRoleEntity
                {
                    UserId = targetUserId,
                    Role = UserRole.Admin,
                    Status = AccountStatus.Active
                };
                await _adminRepository.CreateUserRoleAsync(userRole);
            }
            else
            {
                userRole.Role = UserRole.Admin;
                await _adminRepository.UpdateUserRoleAsync(userRole);
            }
        }

        private static AdminData MapToAdminData(UserRoleEntity userRole)
        {
            return new AdminData
            {
                UserId = userRole.UserId,
                Role = (int)userRole.Role,
                Status = (int)userRole.Status,
                SuspendedByUserId = userRole.SuspendedByUserId,
                SuspendedAt = userRole.SuspendedAt,
                SuspensionReason = userRole.SuspensionReason
            };
        }
    }
}