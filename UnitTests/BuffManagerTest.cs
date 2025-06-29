using GameServer.DB;
using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories;
using GameServer.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class BuffManagerTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly BuffRepository _buffRepository;
        private readonly BuffManager _buffManager;

        public BuffManagerTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _buffRepository = new BuffRepository(_context);
            _buffManager = new BuffManager(_buffRepository);

            // バフマスターデータを初期化
            SetupBuffMasterData();
        }

        private void SetupBuffMasterData()
        {
            var manager = MasterDataManager.Instance;
            manager.Reset();

            var testBuffInfos = new List<BuffInfo>
            {
                new BuffInfo
                {
                    Id = 1,
                    Name = "攻撃力強化",
                    BuffType = BuffType.Buff,
                    MaxStackCount = 5,
                    DefaultDurationSeconds = 300,
                    AttackModifier = 10f,
                    CanStack = true,
                    CanDispel = true
                },
                new BuffInfo
                {
                    Id = 2,
                    Name = "毒",
                    BuffType = BuffType.Debuff,
                    MaxStackCount = 3,
                    DefaultDurationSeconds = 60,
                    StackDecreaseIntervalSeconds = 20,
                    HealthModifier = -5f,
                    CanStack = true,
                    CanDispel = true
                },
                new BuffInfo
                {
                    Id = 3,
                    Name = "永続強化",
                    BuffType = BuffType.Buff,
                    MaxStackCount = 1,
                    DefaultDurationSeconds = -1,
                    DefenseModifier = 20f,
                    CanStack = false,
                    CanDispel = false
                }
            };
            manager.BuffMaster.LoadData(testBuffInfos);
        }

        [Fact]
        public async Task ApplyBuff_ShouldCreateNewBuff_WhenNotExists()
        {
            // Act
            var result = await _buffManager.ApplyBuffAsync(1, 1, 2, 300, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CharacterId);
            Assert.Equal(1, result.BuffMasterId);
            Assert.Equal(2, result.BuffLevel);
            Assert.Equal(1, result.StackCount);
            Assert.Equal(BuffType.Buff, result.BuffType);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task ApplyBuff_ShouldStackBuff_WhenExists()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1, 1, 300, 2);

            // Act
            var result = await _buffManager.ApplyBuffAsync(1, 1, 2, 300, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.StackCount); // 2 + 1
            Assert.Equal(2, result.BuffLevel); // より高いレベルを採用
        }

        [Fact]
        public async Task ApplyBuff_ShouldNotExceedMaxStack()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1, 1, 300, 4);

            // Act
            var result = await _buffManager.ApplyBuffAsync(1, 1, 1, 300, 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.StackCount); // MaxStackCount = 5
        }

        [Fact]
        public async Task ApplyBuff_ShouldCreatePermanentBuff()
        {
            // Act
            var result = await _buffManager.ApplyBuffAsync(1, 3); // 永続強化バフ

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsPermanent());
            Assert.Null(result.EndTime);
            Assert.Equal(-1, result.DurationSeconds);
        }

        [Fact]
        public async Task RemoveBuff_ShouldRemoveCompletely_WhenStackCountZero()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1, 1, 300, 2);

            // Act
            var result = await _buffManager.RemoveBuffAsync(1, 1, 0);

            // Assert
            Assert.True(result);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Empty(buffs);
        }

        [Fact]
        public async Task RemoveBuff_ShouldReduceStack_WhenPartialRemoval()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1, 1, 300, 3);

            // Act
            var result = await _buffManager.RemoveBuffAsync(1, 1, 1);

            // Assert
            Assert.True(result);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Single(buffs);
            Assert.Equal(2, buffs[0].StackCount);
        }

        [Fact]
        public async Task RemoveBuff_ShouldFail_WhenBuffNotDispellable()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 3); // 永続強化（除去不可）

            // Act
            var result = await _buffManager.RemoveBuffAsync(1, 3);

            // Assert
            Assert.False(result);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Single(buffs);
        }

        [Fact]
        public async Task ProcessBuffUpdates_ShouldRemoveExpiredBuffs()
        {
            // Arrange
            var buff = new BuffEntity
            {
                CharacterId = 1,
                BuffMasterId = 1,
                BuffLevel = 1,
                BuffType = BuffType.Buff,
                StackCount = 1,
                StartTime = DateTime.UtcNow.AddMinutes(-10),
                EndTime = DateTime.UtcNow.AddMinutes(-5),
                DurationSeconds = 300
            };
            await _buffRepository.CreateBuffAsync(buff);

            // Act
            var processedCount = await _buffManager.ProcessBuffUpdatesAsync(1);

            // Assert
            Assert.Equal(1, processedCount);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Empty(buffs);
        }

        [Fact]
        public async Task ProcessBuffUpdates_ShouldDecreaseStack()
        {
            // Arrange - 毒バフ（スタック減少あり）
            var buff = new BuffEntity
            {
                CharacterId = 1,
                BuffMasterId = 2,
                BuffLevel = 1,
                BuffType = BuffType.Debuff,
                StackCount = 3,
                StartTime = DateTime.UtcNow.AddMinutes(-1),
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DurationSeconds = 60,
                StackDecreaseIntervalSeconds = 20,
                NextStackDecreaseTime = DateTime.UtcNow.AddSeconds(-1)
            };
            await _buffRepository.CreateBuffAsync(buff);

            // Act
            var processedCount = await _buffManager.ProcessBuffUpdatesAsync(1);

            // Assert
            Assert.Equal(1, processedCount);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Single(buffs);
            Assert.Equal(2, buffs[0].StackCount);
        }

        [Fact]
        public async Task RemoveBuffsByType_ShouldRemoveOnlySpecifiedType()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1); // Buff
            await _buffManager.ApplyBuffAsync(1, 2); // Debuff

            // Act
            var removedCount = await _buffManager.RemoveBuffsByTypeAsync(1, BuffType.Debuff);

            // Assert
            Assert.Equal(1, removedCount);
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);
            Assert.Single(buffs);
            Assert.Equal(BuffType.Buff, buffs[0].BuffType);
        }

        [Fact]
        public async Task CalculateStatusModifiers_ShouldReturnCorrectValues()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1, 2, 300, 3); // 攻撃力+10% * レベル2 * スタック3 = +60%
            await _buffManager.ApplyBuffAsync(1, 3); // 防御力+20%

            // Act
            var modifiers = await _buffManager.CalculateStatusModifiersAsync(1);

            // Assert
            Assert.Equal(60f, modifiers.AttackModifier); // 10 * 2 * 3
            Assert.Equal(20f, modifiers.DefenseModifier); // 20 * 1 * 1
            Assert.Equal(0f, modifiers.HealthModifier);
            Assert.Equal(0f, modifiers.ManaModifier);
            Assert.Equal(0f, modifiers.SpeedModifier);
        }

        [Fact]
        public async Task GetCharacterBuffs_ShouldReturnOnlyActiveBuffs()
        {
            // Arrange
            await _buffManager.ApplyBuffAsync(1, 1);
            await _buffManager.ApplyBuffAsync(1, 2);
            await _buffManager.RemoveBuffAsync(1, 1);

            // Act
            var buffs = await _buffManager.GetCharacterBuffsAsync(1);

            // Assert
            Assert.Single(buffs);
            Assert.Equal(2, buffs[0].BuffMasterId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}