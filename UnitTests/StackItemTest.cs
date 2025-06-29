using GameServer.DB;
using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories;
using GameServer.Services;
using GameServer.UseCases;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class StackItemTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly StackItemRepository _stackItemRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly ScriptExecutor _scriptExecutor;
        private readonly StackItemUseCase _stackItemUseCase;

        public StackItemTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _stackItemRepository = new StackItemRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _scriptExecutor = new ScriptExecutor();
            _stackItemUseCase = new StackItemUseCase(_stackItemRepository, _playerRepository, _scriptExecutor);

            // マスターデータを初期化
            SetupMasterData();
        }

        private void SetupMasterData()
        {
            var manager = MasterDataManager.Instance;
            manager.Reset();

            var testItemInfos = new List<ItemInfo>
            {
                new ItemInfo
                {
                    Id = 1,
                    Name = "体力回復ポーション",
                    ItemType = ItemType.Stack,
                    Price = 100,
                    SellPrice = 50,
                    MaxStackCount = 99,
                    EffectScript = "heal_hp_50"
                },
                new ItemInfo
                {
                    Id = 2,
                    Name = "マナ回復ポーション",
                    ItemType = ItemType.Stack,
                    Price = 80,
                    SellPrice = 40,
                    MaxStackCount = 99,
                    EffectScript = "heal_mp_30"
                },
                new ItemInfo
                {
                    Id = 3,
                    Name = "鉄の剣",
                    ItemType = ItemType.Unique,
                    Price = 500,
                    SellPrice = 250,
                    MaxStackCount = 1
                }
            };
            manager.ItemMaster.LoadData(testItemInfos);
        }

        [Fact]
        public async Task GetPlayerStackItems_ShouldReturnCorrectItems()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem1 = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 5
            };
            var stackItem2 = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 2,
                Count = 3
            };

            await _stackItemRepository.CreateStackItemAsync(stackItem1);
            await _stackItemRepository.CreateStackItemAsync(stackItem2);

            // Act
            var result = await _stackItemUseCase.GetPlayerStackItemsAsync(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => item.ItemName == "体力回復ポーション" && item.Count == 5);
            Assert.Contains(result, item => item.ItemName == "マナ回復ポーション" && item.Count == 3);
        }

        [Fact]
        public async Task UseStackItem_ShouldDecreaseCount_WhenSuccessful()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 5
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.UseStackItemAsync(1, 1, 2);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("HPを50回復しました", result.Message);
            Assert.Equal(3, result.RemainingCount);

            // データベースでも確認
            var updatedItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.NotNull(updatedItem);
            Assert.Equal(3, updatedItem.Count);
        }

        [Fact]
        public async Task UseStackItem_ShouldDeleteItem_WhenCountReachesZero()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 2
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.UseStackItemAsync(1, 1, 2);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(0, result.RemainingCount);

            // アイテムが削除されていることを確認
            var deletedItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.Null(deletedItem);
        }

        [Fact]
        public async Task UseStackItem_ShouldFail_WhenInsufficientCount()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 1
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.UseStackItemAsync(1, 1, 5);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("アイテムが不足しています", result.Message);
        }

        [Fact]
        public async Task PurchaseStackItem_ShouldCreateNewItem_WhenNotExists()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            // Act
            var result = await _stackItemUseCase.PurchaseStackItemAsync(1, 1, 3);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("体力回復ポーションを3個購入しました", result.Message);
            Assert.Equal(300, result.TotalPrice);
            Assert.Equal(700, result.RemainingMoney);

            // アイテムが作成されていることを確認
            var stackItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.NotNull(stackItem);
            Assert.Equal(3, stackItem.Count);

            // プレイヤーの所持金が減っていることを確認
            var updatedPlayer = await _playerRepository.GetPlayerAsync(1);
            Assert.NotNull(updatedPlayer);
            Assert.Equal(700, updatedPlayer.Money);
        }

        [Fact]
        public async Task PurchaseStackItem_ShouldIncreaseCount_WhenAlreadyExists()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var existingItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 5
            };
            await _stackItemRepository.CreateStackItemAsync(existingItem);

            // Act
            var result = await _stackItemUseCase.PurchaseStackItemAsync(1, 1, 2);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.TotalPrice);

            // 個数が増えていることを確認
            var updatedItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.NotNull(updatedItem);
            Assert.Equal(7, updatedItem.Count);
        }

        [Fact]
        public async Task PurchaseStackItem_ShouldFail_WhenInsufficientMoney()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 50
            };
            await _playerRepository.CreatePlayerAsync(player);

            // Act
            var result = await _stackItemUseCase.PurchaseStackItemAsync(1, 1, 1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("お金が不足しています", result.Message);
        }

        [Fact]
        public async Task PurchaseStackItem_ShouldFail_WhenStackLimitExceeded()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 10000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var existingItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 98
            };
            await _stackItemRepository.CreateStackItemAsync(existingItem);

            // Act
            var result = await _stackItemUseCase.PurchaseStackItemAsync(1, 1, 5);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("スタック上限を超えています", result.Message);
        }

        [Fact]
        public async Task SellStackItem_ShouldIncreaseMoneyAndDecreaseCount()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 5
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.SellStackItemAsync(1, 1, 2);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("体力回復ポーションを2個売却しました", result.Message);
            Assert.Equal(100, result.TotalSellPrice); // 50 * 2
            Assert.Equal(1100, result.RemainingMoney);
            Assert.Equal(3, result.RemainingCount);

            // データベースでも確認
            var updatedItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.NotNull(updatedItem);
            Assert.Equal(3, updatedItem.Count);

            var updatedPlayer = await _playerRepository.GetPlayerAsync(1);
            Assert.NotNull(updatedPlayer);
            Assert.Equal(1100, updatedPlayer.Money);
        }

        [Fact]
        public async Task SellStackItem_ShouldDeleteItem_WhenAllSold()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 3
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.SellStackItemAsync(1, 1, 3);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(150, result.TotalSellPrice); // 50 * 3
            Assert.Equal(0, result.RemainingCount);

            // アイテムが削除されていることを確認
            var deletedItem = await _stackItemRepository.GetStackItemAsync(1, 1);
            Assert.Null(deletedItem);
        }

        [Fact]
        public async Task SellStackItem_ShouldFail_WhenInsufficientCount()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var stackItem = new StackItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Count = 1
            };
            await _stackItemRepository.CreateStackItemAsync(stackItem);

            // Act
            var result = await _stackItemUseCase.SellStackItemAsync(1, 1, 5);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("アイテムが不足しています", result.Message);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}