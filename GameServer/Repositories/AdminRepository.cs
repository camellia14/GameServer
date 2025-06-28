using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserRoleEntity?> GetUserRoleAsync(int userId)
        {
            return await _context.UserRoles
                .Include(ur => ur.Player)
                .FirstOrDefaultAsync(ur => ur.UserId == userId);
        }

        public async Task<UserRoleEntity> CreateUserRoleAsync(UserRoleEntity userRole)
        {
            userRole.CreatedAt = DateTime.UtcNow;
            userRole.UpdatedAt = DateTime.UtcNow;
            
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task<UserRoleEntity> UpdateUserRoleAsync(UserRoleEntity userRole)
        {
            userRole.UpdatedAt = DateTime.UtcNow;
            
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId);
            
            return userRole?.Role == UserRole.Admin && userRole.Status == AccountStatus.Active;
        }

        public async Task<List<UserRoleEntity>> GetSuspendedUsersAsync()
        {
            return await _context.UserRoles
                .Include(ur => ur.Player)
                .Include(ur => ur.SuspendedByUser)
                .Where(ur => ur.Status == AccountStatus.Suspended)
                .ToListAsync();
        }

        public async Task SuspendUserAsync(int targetUserId, int adminUserId, string reason)
        {
            var userRole = await GetUserRoleAsync(targetUserId);
            if (userRole == null)
            {
                userRole = new UserRoleEntity
                {
                    UserId = targetUserId,
                    Role = UserRole.Player,
                    Status = AccountStatus.Suspended,
                    SuspendedByUserId = adminUserId,
                    SuspendedAt = DateTime.UtcNow,
                    SuspensionReason = reason
                };
                await CreateUserRoleAsync(userRole);
            }
            else
            {
                userRole.Status = AccountStatus.Suspended;
                userRole.SuspendedByUserId = adminUserId;
                userRole.SuspendedAt = DateTime.UtcNow;
                userRole.SuspensionReason = reason;
                await UpdateUserRoleAsync(userRole);
            }
        }

        public async Task ReactivateUserAsync(int targetUserId, int adminUserId)
        {
            var userRole = await GetUserRoleAsync(targetUserId);
            if (userRole != null)
            {
                userRole.Status = AccountStatus.Active;
                userRole.SuspendedByUserId = null;
                userRole.SuspendedAt = null;
                userRole.SuspensionReason = null;
                await UpdateUserRoleAsync(userRole);
            }
        }

        public async Task DeleteUserAsync(int targetUserId, int adminUserId)
        {
            var userRole = await GetUserRoleAsync(targetUserId);
            if (userRole == null)
            {
                userRole = new UserRoleEntity
                {
                    UserId = targetUserId,
                    Role = UserRole.Player,
                    Status = AccountStatus.Deleted,
                    SuspendedByUserId = adminUserId,
                    SuspendedAt = DateTime.UtcNow
                };
                await CreateUserRoleAsync(userRole);
            }
            else
            {
                userRole.Status = AccountStatus.Deleted;
                userRole.SuspendedByUserId = adminUserId;
                userRole.SuspendedAt = DateTime.UtcNow;
                await UpdateUserRoleAsync(userRole);
            }
        }
    }
}