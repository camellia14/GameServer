using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories;
using GameServer.UseCases;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class AdminTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AdminRepository _adminRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly AdminUseCase _adminUseCase;

        public AdminTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _adminRepository = new AdminRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _adminUseCase = new AdminUseCase(_adminRepository, _playerRepository);
        }

        [Fact]
        public async Task IsAdmin_ShouldReturnFalse_WhenUserIsNotAdmin()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "RegularPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var isAdmin = await _adminUseCase.IsAdminAsync(player.UserId);

            Assert.False(isAdmin);
        }

        [Fact]
        public async Task PromoteToAdmin_ShouldMakeUserAdmin()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            var targetUser = new PlayerEntity
            {
                UserId = 2,
                UserName = "TargetUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(targetUser);

            // Make first user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            await _adminUseCase.PromoteToAdminAsync(targetUser.UserId, admin.UserId);

            var isTargetAdmin = await _adminUseCase.IsAdminAsync(targetUser.UserId);
            Assert.True(isTargetAdmin);
        }

        [Fact]
        public async Task SuspendUser_ShouldSuspendUser_WhenAdminPerformsAction()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            var targetUser = new PlayerEntity
            {
                UserId = 2,
                UserName = "TargetUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(targetUser);

            // Make first user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            await _adminUseCase.SuspendUserAsync(targetUser.UserId, admin.UserId, "Violation of terms");

            var userRole = await _adminRepository.GetUserRoleAsync(targetUser.UserId);
            Assert.NotNull(userRole);
            Assert.Equal(AccountStatus.Suspended, userRole.Status);
            Assert.Equal("Violation of terms", userRole.SuspensionReason);
            Assert.Equal(admin.UserId, userRole.SuspendedByUserId);
        }

        [Fact]
        public async Task SuspendUser_ShouldThrowException_WhenNonAdminTriesToSuspend()
        {
            var regularUser = new PlayerEntity
            {
                UserId = 1,
                UserName = "RegularUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(regularUser);

            var targetUser = new PlayerEntity
            {
                UserId = 2,
                UserName = "TargetUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(targetUser);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await _adminUseCase.SuspendUserAsync(targetUser.UserId, regularUser.UserId, "Should fail");
            });
        }

        [Fact]
        public async Task SuspendUser_ShouldThrowException_WhenAdminTriesToSuspendSelf()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            // Make user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _adminUseCase.SuspendUserAsync(admin.UserId, admin.UserId, "Self-suspension");
            });
        }

        [Fact]
        public async Task ReactivateUser_ShouldReactivateSuspendedUser()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            var targetUser = new PlayerEntity
            {
                UserId = 2,
                UserName = "TargetUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(targetUser);

            // Make first user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            // Suspend user first
            await _adminUseCase.SuspendUserAsync(targetUser.UserId, admin.UserId, "Test suspension");

            // Reactivate user
            await _adminUseCase.ReactivateUserAsync(targetUser.UserId, admin.UserId);

            var userRole = await _adminRepository.GetUserRoleAsync(targetUser.UserId);
            Assert.NotNull(userRole);
            Assert.Equal(AccountStatus.Active, userRole.Status);
            Assert.Null(userRole.SuspensionReason);
            Assert.Null(userRole.SuspendedByUserId);
        }

        [Fact]
        public async Task DeleteUser_ShouldMarkUserAsDeleted()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            var targetUser = new PlayerEntity
            {
                UserId = 2,
                UserName = "TargetUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(targetUser);

            // Make first user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            await _adminUseCase.DeleteUserAsync(targetUser.UserId, admin.UserId);

            var userRole = await _adminRepository.GetUserRoleAsync(targetUser.UserId);
            Assert.NotNull(userRole);
            Assert.Equal(AccountStatus.Deleted, userRole.Status);
            Assert.Equal(admin.UserId, userRole.SuspendedByUserId);
        }

        [Fact]
        public async Task GetSuspendedUsers_ShouldReturnOnlySuspendedUsers()
        {
            var admin = new PlayerEntity
            {
                UserId = 1,
                UserName = "AdminUser",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(admin);

            var user1 = new PlayerEntity { UserId = 2, UserName = "User1", Money = 1000 };
            var user2 = new PlayerEntity { UserId = 3, UserName = "User2", Money = 1000 };
            var user3 = new PlayerEntity { UserId = 4, UserName = "User3", Money = 1000 };

            await _playerRepository.CreatePlayerAsync(user1);
            await _playerRepository.CreatePlayerAsync(user2);
            await _playerRepository.CreatePlayerAsync(user3);

            // Make first user admin
            var adminRole = new UserRoleEntity
            {
                UserId = admin.UserId,
                Role = UserRole.Admin,
                Status = AccountStatus.Active
            };
            await _adminRepository.CreateUserRoleAsync(adminRole);

            // Suspend two users
            await _adminUseCase.SuspendUserAsync(user1.UserId, admin.UserId, "Reason 1");
            await _adminUseCase.SuspendUserAsync(user3.UserId, admin.UserId, "Reason 3");

            var suspendedUsers = await _adminUseCase.GetSuspendedUsersAsync(admin.UserId);

            Assert.Equal(2, suspendedUsers.Count);
            Assert.Contains(suspendedUsers, u => u.UserId == user1.UserId);
            Assert.Contains(suspendedUsers, u => u.UserId == user3.UserId);
            Assert.DoesNotContain(suspendedUsers, u => u.UserId == user2.UserId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}