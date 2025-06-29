using GameServer.DB;
using GameServer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class FriendTest : IDisposable
    {
        private readonly AppDbContext _context;

        public FriendTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
        }

        [Fact]
        public void FriendEntity_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                RequestMessage = "Let's be friends!"
            };

            // Assert
            Assert.Equal(1, friend.RequesterUserId);
            Assert.Equal(2, friend.AddresseeUserId);
            Assert.Equal(FriendStatus.Pending, friend.Status);
            Assert.Equal("Let's be friends!", friend.RequestMessage);
            Assert.False(friend.IsFavorite);
            Assert.False(friend.IsBlocked);
            Assert.Null(friend.RespondedAt);
        }

        [Fact]
        public void Accept_ShouldChangeStatusToAccepted()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Pending
            };

            // Act
            var result = friend.Accept();

            // Assert
            Assert.True(result);
            Assert.Equal(FriendStatus.Accepted, friend.Status);
            Assert.NotNull(friend.RespondedAt);
        }

        [Fact]
        public void Accept_ShouldFail_WhenNotPending()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Accepted
            };

            // Act
            var result = friend.Accept();

            // Assert
            Assert.False(result);
            Assert.Equal(FriendStatus.Accepted, friend.Status);
        }

        [Fact]
        public void Reject_ShouldChangeStatusToRejected()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Pending
            };

            // Act
            var result = friend.Reject();

            // Assert
            Assert.True(result);
            Assert.Equal(FriendStatus.Rejected, friend.Status);
            Assert.NotNull(friend.RespondedAt);
        }

        [Fact]
        public void Reject_ShouldFail_WhenNotPending()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Rejected
            };

            // Act
            var result = friend.Reject();

            // Assert
            Assert.False(result);
            Assert.Equal(FriendStatus.Rejected, friend.Status);
        }

        [Fact]
        public void Remove_ShouldChangeStatusToRemoved()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Accepted
            };

            // Act
            var result = friend.Remove();

            // Assert
            Assert.True(result);
            Assert.Equal(FriendStatus.Removed, friend.Status);
        }

        [Fact]
        public void Remove_ShouldFail_WhenNotAccepted()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Pending
            };

            // Act
            var result = friend.Remove();

            // Assert
            Assert.False(result);
            Assert.Equal(FriendStatus.Pending, friend.Status);
        }

        [Fact]
        public void Block_ShouldSetIsBlockedToTrue()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                IsBlocked = false
            };

            // Act
            friend.Block();

            // Assert
            Assert.True(friend.IsBlocked);
        }

        [Fact]
        public void Unblock_ShouldSetIsBlockedToFalse()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                IsBlocked = true
            };

            // Act
            friend.Unblock();

            // Assert
            Assert.False(friend.IsBlocked);
        }

        [Fact]
        public void ToggleFavorite_ShouldSwitchFavoriteStatus()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                IsFavorite = false
            };

            // Act
            friend.ToggleFavorite();

            // Assert
            Assert.True(friend.IsFavorite);

            // Act again
            friend.ToggleFavorite();

            // Assert
            Assert.False(friend.IsFavorite);
        }

        [Fact]
        public void UpdateLastPlayedTogether_ShouldSetCurrentTime()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                LastPlayedTogetherAt = null
            };
            var beforeUpdate = DateTime.UtcNow;

            // Act
            friend.UpdateLastPlayedTogether();

            // Assert
            var afterUpdate = DateTime.UtcNow;
            Assert.NotNull(friend.LastPlayedTogetherAt);
            Assert.True(friend.LastPlayedTogetherAt >= beforeUpdate);
            Assert.True(friend.LastPlayedTogetherAt <= afterUpdate);
        }

        [Fact]
        public void GetOtherUserId_ShouldReturnCorrectUserId()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2
            };

            // Act & Assert
            Assert.Equal(2, friend.GetOtherUserId(1));
            Assert.Equal(1, friend.GetOtherUserId(2));
        }

        [Fact]
        public void IsRequester_ShouldReturnCorrectResult()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2
            };

            // Act & Assert
            Assert.True(friend.IsRequester(1));
            Assert.False(friend.IsRequester(2));
        }

        [Fact]
        public async Task FriendEntity_ShouldSaveToDatabase()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Pending,
                RequestMessage = "Hello!"
            };

            // Act
            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();

            // Assert
            var savedFriend = await _context.Friends.FirstOrDefaultAsync(f => f.RequesterUserId == 1 && f.AddresseeUserId == 2);
            Assert.NotNull(savedFriend);
            Assert.Equal(FriendStatus.Pending, savedFriend.Status);
            Assert.Equal("Hello!", savedFriend.RequestMessage);
        }

        [Fact]
        public async Task FriendEntity_ShouldUpdateInDatabase()
        {
            // Arrange
            var friend = new FriendEntity
            {
                RequesterUserId = 1,
                AddresseeUserId = 2,
                Status = FriendStatus.Pending
            };
            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();

            // Act
            friend.Accept();
            await _context.SaveChangesAsync();

            // Assert
            var updatedFriend = await _context.Friends.FirstOrDefaultAsync(f => f.FriendId == friend.FriendId);
            Assert.NotNull(updatedFriend);
            Assert.Equal(FriendStatus.Accepted, updatedFriend.Status);
            Assert.NotNull(updatedFriend.RespondedAt);
        }

        [Fact]
        public void FriendStatus_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)FriendStatus.Pending);
            Assert.Equal(1, (int)FriendStatus.Accepted);
            Assert.Equal(2, (int)FriendStatus.Rejected);
            Assert.Equal(3, (int)FriendStatus.Removed);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}