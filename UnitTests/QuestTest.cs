using GameServer.DB;
using GameServer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class QuestTest : IDisposable
    {
        private readonly AppDbContext _context;

        public QuestTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
        }

        [Fact]
        public void QuestEntity_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 10
            };

            // Assert
            Assert.Equal(1, quest.PlayerUserId);
            Assert.Equal(101, quest.QuestMasterId);
            Assert.Equal(QuestStatus.InProgress, quest.Status);
            Assert.Equal(0, quest.CurrentProgress);
            Assert.Equal(10, quest.TargetProgress);
            Assert.False(quest.IsCompleted());
            Assert.Equal(0f, quest.GetProgressRatio());
        }

        [Fact]
        public void AddProgress_ShouldUpdateCurrentProgress()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 10,
                Status = QuestStatus.InProgress
            };

            // Act
            var completed = quest.AddProgress(3);

            // Assert
            Assert.Equal(3, quest.CurrentProgress);
            Assert.Equal(0.3f, quest.GetProgressRatio());
            Assert.False(completed);
            Assert.Equal(QuestStatus.InProgress, quest.Status);
        }

        [Fact]
        public void AddProgress_ShouldCompleteQuest_WhenTargetReached()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 5,
                CurrentProgress = 3,
                Status = QuestStatus.InProgress
            };

            // Act
            var completed = quest.AddProgress(3);

            // Assert
            Assert.True(completed);
            Assert.Equal(5, quest.CurrentProgress);
            Assert.Equal(1.0f, quest.GetProgressRatio());
            Assert.Equal(QuestStatus.Completed, quest.Status);
            Assert.NotNull(quest.CompletedAt);
            Assert.True(quest.IsCompleted());
        }

        [Fact]
        public void AddProgress_ShouldNotExceedTarget()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 5,
                CurrentProgress = 3,
                Status = QuestStatus.InProgress
            };

            // Act
            quest.AddProgress(10);

            // Assert
            Assert.Equal(5, quest.CurrentProgress);
            Assert.Equal(1.0f, quest.GetProgressRatio());
        }

        [Fact]
        public void ClaimReward_ShouldSucceed_WhenQuestCompleted()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                Status = QuestStatus.Completed
            };

            // Act
            var result = quest.ClaimReward();

            // Assert
            Assert.True(result);
            Assert.Equal(QuestStatus.RewardClaimed, quest.Status);
            Assert.NotNull(quest.RewardClaimedAt);
            Assert.True(quest.IsCompleted());
        }

        [Fact]
        public void ClaimReward_ShouldFail_WhenQuestNotCompleted()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                Status = QuestStatus.InProgress
            };

            // Act
            var result = quest.ClaimReward();

            // Assert
            Assert.False(result);
            Assert.Equal(QuestStatus.InProgress, quest.Status);
            Assert.Null(quest.RewardClaimedAt);
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenPastExpiry()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                ExpiresAt = DateTime.UtcNow.AddMinutes(-10)
            };

            // Act & Assert
            Assert.True(quest.IsExpired());
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenNoExpiry()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                ExpiresAt = null
            };

            // Act & Assert
            Assert.False(quest.IsExpired());
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenNotExpired()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            // Act & Assert
            Assert.False(quest.IsExpired());
        }

        [Fact]
        public async Task QuestEntity_ShouldSaveToDatabase()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 10,
                Status = QuestStatus.InProgress
            };

            // Act
            _context.Quests.Add(quest);
            await _context.SaveChangesAsync();

            // Assert
            var savedQuest = await _context.Quests.FirstOrDefaultAsync(q => q.QuestMasterId == 101);
            Assert.NotNull(savedQuest);
            Assert.Equal(1, savedQuest.PlayerUserId);
            Assert.Equal(101, savedQuest.QuestMasterId);
            Assert.Equal(QuestStatus.InProgress, savedQuest.Status);
        }

        [Fact]
        public async Task QuestEntity_ShouldUpdateInDatabase()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 10,
                Status = QuestStatus.InProgress
            };
            _context.Quests.Add(quest);
            await _context.SaveChangesAsync();

            // Act
            quest.AddProgress(5);
            await _context.SaveChangesAsync();

            // Assert
            var updatedQuest = await _context.Quests.FirstOrDefaultAsync(q => q.QuestId == quest.QuestId);
            Assert.NotNull(updatedQuest);
            Assert.Equal(5, updatedQuest.CurrentProgress);
            Assert.Equal(QuestStatus.InProgress, updatedQuest.Status);
        }

        [Fact]
        public void GetProgressRatio_ShouldHandleZeroTarget()
        {
            // Arrange
            var quest = new QuestEntity
            {
                PlayerUserId = 1,
                QuestMasterId = 101,
                TargetProgress = 0,
                CurrentProgress = 5
            };

            // Act & Assert
            Assert.Equal(1.0f, quest.GetProgressRatio());
        }

        [Fact]
        public void QuestStatus_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)QuestStatus.InProgress);
            Assert.Equal(1, (int)QuestStatus.Completed);
            Assert.Equal(2, (int)QuestStatus.RewardClaimed);
            Assert.Equal(3, (int)QuestStatus.Abandoned);
            Assert.Equal(4, (int)QuestStatus.Expired);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}