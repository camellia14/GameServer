using GameServer.DB;
using GameServer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class RoomTest : IDisposable
    {
        private readonly AppDbContext _context;

        public RoomTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
        }

        [Fact]
        public void RoomEntity_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                Description = "A test room",
                OwnerUserId = 1,
                MaxParticipants = 10
            };

            // Assert
            Assert.Equal("Test Room", room.RoomName);
            Assert.Equal("A test room", room.Description);
            Assert.Equal(1, room.OwnerUserId);
            Assert.Equal(10, room.MaxParticipants);
            Assert.Equal(0, room.CurrentParticipants);
            Assert.Equal(RoomStatus.Active, room.Status);
            Assert.Equal(RoomType.Public, room.RoomType);
            Assert.False(room.IsPrivate);
            Assert.False(room.IsFriendsOnly);
            Assert.False(room.IsDeleted);
            Assert.Null(room.Password);
        }

        [Fact]
        public void IsPasswordProtected_ShouldReturnTrue_WhenPasswordIsSet()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Protected Room",
                OwnerUserId = 1,
                Password = "secret123"
            };

            // Act & Assert
            Assert.True(room.IsPasswordProtected());
        }

        [Fact]
        public void IsPasswordProtected_ShouldReturnFalse_WhenPasswordIsNull()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Open Room",
                OwnerUserId = 1,
                Password = null
            };

            // Act & Assert
            Assert.False(room.IsPasswordProtected());
        }

        [Fact]
        public void IsFull_ShouldReturnTrue_WhenCurrentParticipantsEqualMax()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Full Room",
                OwnerUserId = 1,
                MaxParticipants = 5,
                CurrentParticipants = 5
            };

            // Act & Assert
            Assert.True(room.IsFull());
        }

        [Fact]
        public void IsFull_ShouldReturnFalse_WhenCurrentParticipantsLessThanMax()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Available Room",
                OwnerUserId = 1,
                MaxParticipants = 5,
                CurrentParticipants = 3
            };

            // Act & Assert
            Assert.False(room.IsFull());
        }

        [Fact]
        public void IsJoinable_ShouldReturnTrue_WhenActiveAndNotFullAndNotDeleted()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Joinable Room",
                OwnerUserId = 1,
                Status = RoomStatus.Active,
                MaxParticipants = 5,
                CurrentParticipants = 3,
                IsDeleted = false
            };

            // Act & Assert
            Assert.True(room.IsJoinable());
        }

        [Fact]
        public void IsJoinable_ShouldReturnFalse_WhenDeleted()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Deleted Room",
                OwnerUserId = 1,
                Status = RoomStatus.Active,
                MaxParticipants = 5,
                CurrentParticipants = 3,
                IsDeleted = true
            };

            // Act & Assert
            Assert.False(room.IsJoinable());
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenCorrectPassword()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Protected Room",
                OwnerUserId = 1,
                Password = "secret123"
            };

            // Act & Assert
            Assert.True(room.VerifyPassword("secret123"));
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenIncorrectPassword()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Protected Room",
                OwnerUserId = 1,
                Password = "secret123"
            };

            // Act & Assert
            Assert.False(room.VerifyPassword("wrongpassword"));
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenNoPasswordRequired()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Open Room",
                OwnerUserId = 1,
                Password = null
            };

            // Act & Assert
            Assert.True(room.VerifyPassword("anypassword"));
            Assert.True(room.VerifyPassword(null));
        }

        [Fact]
        public void UpdateRoomInfo_ShouldUpdateAllFields()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Old Name",
                Description = "Old Description",
                OwnerUserId = 1,
                MaxParticipants = 5,
                Password = null
            };

            // Act
            room.UpdateRoomInfo("New Name", "New Description", 10, "newpassword");

            // Assert
            Assert.Equal("New Name", room.RoomName);
            Assert.Equal("New Description", room.Description);
            Assert.Equal(10, room.MaxParticipants);
            Assert.Equal("newpassword", room.Password);
        }

        [Fact]
        public void UpdateRoomInfo_ShouldNotUpdateMaxParticipants_WhenLessThanCurrent()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                MaxParticipants = 10,
                CurrentParticipants = 5
            };

            // Act
            room.UpdateRoomInfo(maxParticipants: 3);

            // Assert
            Assert.Equal(10, room.MaxParticipants); // Should not change
        }

        [Fact]
        public void IncrementParticipants_ShouldIncreaseCount_WhenNotFull()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                MaxParticipants = 5,
                CurrentParticipants = 3
            };

            // Act
            var result = room.IncrementParticipants();

            // Assert
            Assert.True(result);
            Assert.Equal(4, room.CurrentParticipants);
        }

        [Fact]
        public void IncrementParticipants_ShouldFail_WhenFull()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Full Room",
                OwnerUserId = 1,
                MaxParticipants = 5,
                CurrentParticipants = 5
            };

            // Act
            var result = room.IncrementParticipants();

            // Assert
            Assert.False(result);
            Assert.Equal(5, room.CurrentParticipants);
        }

        [Fact]
        public void DecrementParticipants_ShouldDecreaseCount_WhenGreaterThanZero()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                CurrentParticipants = 3
            };

            // Act
            var result = room.DecrementParticipants();

            // Assert
            Assert.True(result);
            Assert.Equal(2, room.CurrentParticipants);
        }

        [Fact]
        public void DecrementParticipants_ShouldFail_WhenZero()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Empty Room",
                OwnerUserId = 1,
                CurrentParticipants = 0
            };

            // Act
            var result = room.DecrementParticipants();

            // Assert
            Assert.False(result);
            Assert.Equal(0, room.CurrentParticipants);
        }

        [Fact]
        public void Delete_ShouldSetDeletedAndClosed()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                Status = RoomStatus.Active,
                IsDeleted = false
            };

            // Act
            room.Delete();

            // Assert
            Assert.True(room.IsDeleted);
            Assert.Equal(RoomStatus.Closed, room.Status);
        }

        [Fact]
        public void Close_ShouldSetStatusToClosed()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                Status = RoomStatus.Active
            };

            // Act
            room.Close();

            // Assert
            Assert.Equal(RoomStatus.Closed, room.Status);
        }

        [Fact]
        public void Reopen_ShouldSetStatusToActive_WhenNotDeleted()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                Status = RoomStatus.Closed,
                IsDeleted = false
            };

            // Act
            room.Reopen();

            // Assert
            Assert.Equal(RoomStatus.Active, room.Status);
        }

        [Fact]
        public void Reopen_ShouldNotSetStatusToActive_WhenDeleted()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Deleted Room",
                OwnerUserId = 1,
                Status = RoomStatus.Closed,
                IsDeleted = true
            };

            // Act
            room.Reopen();

            // Assert
            Assert.Equal(RoomStatus.Closed, room.Status); // Should not change
        }

        [Fact]
        public void UpdateActivity_ShouldSetLastActivityTime()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Test Room",
                OwnerUserId = 1,
                LastActivityAt = null
            };
            var beforeUpdate = DateTime.UtcNow;

            // Act
            room.UpdateActivity();

            // Assert
            var afterUpdate = DateTime.UtcNow;
            Assert.NotNull(room.LastActivityAt);
            Assert.True(room.LastActivityAt >= beforeUpdate);
            Assert.True(room.LastActivityAt <= afterUpdate);
        }

        [Fact]
        public async Task RoomEntity_ShouldSaveToDatabase()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Database Test Room",
                Description = "Testing database operations",
                OwnerUserId = 1,
                MaxParticipants = 8
            };

            // Act
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Assert
            var savedRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomName == "Database Test Room");
            Assert.NotNull(savedRoom);
            Assert.Equal("Testing database operations", savedRoom.Description);
            Assert.Equal(1, savedRoom.OwnerUserId);
            Assert.Equal(8, savedRoom.MaxParticipants);
        }

        [Fact]
        public async Task RoomEntity_ShouldUpdateInDatabase()
        {
            // Arrange
            var room = new RoomEntity
            {
                RoomName = "Original Room",
                OwnerUserId = 1,
                Status = RoomStatus.Active,
                CurrentParticipants = 0
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Act
            room.UpdateRoomInfo("Updated Room", "Updated description");
            room.IncrementParticipants();
            await _context.SaveChangesAsync();

            // Assert
            var updatedRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == room.RoomId);
            Assert.NotNull(updatedRoom);
            Assert.Equal("Updated Room", updatedRoom.RoomName);
            Assert.Equal("Updated description", updatedRoom.Description);
            Assert.Equal(1, updatedRoom.CurrentParticipants);
        }

        [Fact]
        public void RoomStatus_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)RoomStatus.Active);
            Assert.Equal(1, (int)RoomStatus.Paused);
            Assert.Equal(2, (int)RoomStatus.Closed);
            Assert.Equal(3, (int)RoomStatus.InGame);
        }

        [Fact]
        public void RoomType_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)RoomType.Public);
            Assert.Equal(1, (int)RoomType.Private);
            Assert.Equal(2, (int)RoomType.FriendsOnly);
            Assert.Equal(3, (int)RoomType.InviteOnly);
        }

        // RoomMemberEntity Tests

        [Fact]
        public void RoomMemberEntity_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var member = new RoomMemberEntity
            {
                RoomId = 1,
                UserId = 2,
                Role = RoomMemberRole.Member
            };

            // Assert
            Assert.Equal(1, member.RoomId);
            Assert.Equal(2, member.UserId);
            Assert.Equal(RoomMemberRole.Member, member.Role);
            Assert.Equal(RoomMemberStatus.Active, member.Status);
            Assert.Null(member.LeftAt);
            Assert.Null(member.BannedAt);
        }

        [Fact]
        public void Leave_ShouldSetStatusToLeft()
        {
            // Arrange
            var member = new RoomMemberEntity
            {
                RoomId = 1,
                UserId = 2,
                Status = RoomMemberStatus.Active
            };
            var beforeLeave = DateTime.UtcNow;

            // Act
            member.Leave();

            // Assert
            var afterLeave = DateTime.UtcNow;
            Assert.Equal(RoomMemberStatus.Left, member.Status);
            Assert.NotNull(member.LeftAt);
            Assert.True(member.LeftAt >= beforeLeave);
            Assert.True(member.LeftAt <= afterLeave);
        }

        [Fact]
        public void Ban_ShouldSetStatusToBanned()
        {
            // Arrange
            var member = new RoomMemberEntity
            {
                RoomId = 1,
                UserId = 2,
                Status = RoomMemberStatus.Active
            };
            var beforeBan = DateTime.UtcNow;

            // Act
            member.Ban(3, "Spam");

            // Assert
            var afterBan = DateTime.UtcNow;
            Assert.Equal(RoomMemberStatus.Banned, member.Status);
            Assert.NotNull(member.BannedAt);
            Assert.Equal(3, member.BannedByUserId);
            Assert.Equal("Spam", member.BanReason);
            Assert.True(member.BannedAt >= beforeBan);
            Assert.True(member.BannedAt <= afterBan);
        }

        [Fact]
        public void Unban_ShouldClearBanStatus()
        {
            // Arrange
            var member = new RoomMemberEntity
            {
                RoomId = 1,
                UserId = 2,
                Status = RoomMemberStatus.Banned,
                BannedAt = DateTime.UtcNow,
                BannedByUserId = 3,
                BanReason = "Spam"
            };

            // Act
            member.Unban();

            // Assert
            Assert.Equal(RoomMemberStatus.Active, member.Status);
            Assert.Null(member.BannedAt);
            Assert.Null(member.BannedByUserId);
            Assert.Null(member.BanReason);
        }

        [Fact]
        public void ChangeRole_ShouldUpdateRole()
        {
            // Arrange
            var member = new RoomMemberEntity
            {
                RoomId = 1,
                UserId = 2,
                Role = RoomMemberRole.Member,
                Status = RoomMemberStatus.Active
            };

            // Act
            member.ChangeRole(RoomMemberRole.Moderator);

            // Assert
            Assert.Equal(RoomMemberRole.Moderator, member.Role);
        }

        [Fact]
        public void HasAdminRights_ShouldReturnTrue_ForOwnerAndAdmin()
        {
            // Arrange
            var owner = new RoomMemberEntity { Role = RoomMemberRole.Owner };
            var admin = new RoomMemberEntity { Role = RoomMemberRole.Admin };
            var moderator = new RoomMemberEntity { Role = RoomMemberRole.Moderator };
            var member = new RoomMemberEntity { Role = RoomMemberRole.Member };

            // Act & Assert
            Assert.True(owner.HasAdminRights());
            Assert.True(admin.HasAdminRights());
            Assert.False(moderator.HasAdminRights());
            Assert.False(member.HasAdminRights());
        }

        [Fact]
        public void HasModeratorRights_ShouldReturnTrue_ForModeratorAndAbove()
        {
            // Arrange
            var owner = new RoomMemberEntity { Role = RoomMemberRole.Owner };
            var admin = new RoomMemberEntity { Role = RoomMemberRole.Admin };
            var moderator = new RoomMemberEntity { Role = RoomMemberRole.Moderator };
            var member = new RoomMemberEntity { Role = RoomMemberRole.Member };

            // Act & Assert
            Assert.True(owner.HasModeratorRights());
            Assert.True(admin.HasModeratorRights());
            Assert.True(moderator.HasModeratorRights());
            Assert.False(member.HasModeratorRights());
        }

        [Fact]
        public void RoomMemberRole_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)RoomMemberRole.Member);
            Assert.Equal(1, (int)RoomMemberRole.Moderator);
            Assert.Equal(2, (int)RoomMemberRole.Admin);
            Assert.Equal(3, (int)RoomMemberRole.Owner);
        }

        [Fact]
        public void RoomMemberStatus_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)RoomMemberStatus.Active);
            Assert.Equal(1, (int)RoomMemberStatus.Left);
            Assert.Equal(2, (int)RoomMemberStatus.Kicked);
            Assert.Equal(3, (int)RoomMemberStatus.Banned);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}