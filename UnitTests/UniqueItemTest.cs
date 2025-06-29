using GameServer.DB;
using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories;
using GameServer.UseCases;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class UniqueItemTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UniqueItemRepository _uniqueItemRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly UniqueItemUseCase _uniqueItemUseCase;

        public UniqueItemTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _uniqueItemRepository = new UniqueItemRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _uniqueItemUseCase = new UniqueItemUseCase(_uniqueItemRepository, _playerRepository);

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
                    Name = "鉄の剣",
                    ItemType = ItemType.Unique,
                    Price = 500,
                    SellPrice = 250,
                    MaxStackCount = 1,
                    Rarity = 2
                },
                new ItemInfo
                {
                    Id = 2,
                    Name = "レザーアーマー",
                    ItemType = ItemType.Unique,
                    Price = 400,
                    SellPrice = 200,
                    MaxStackCount = 1,
                    Rarity = 2
                },
                new ItemInfo
                {
                    Id = 3,
                    Name = "ドラゴンソード",
                    ItemType = ItemType.Unique,
                    Price = 10000,
                    SellPrice = 5000,
                    MaxStackCount = 1,
                    Rarity = 5
                }
            };
            manager.ItemMaster.LoadData(testItemInfos);
        }

        [Fact]
        public async Task GetPlayerUniqueItems_ShouldReturnCorrectItems()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem1 = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                EnhancementLevel = 2,
                AttackBonus = 10,
                Status = ItemStatus.Inventory
            };
            var uniqueItem2 = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 2,
                EnhancementLevel = 0,
                DefenseBonus = 5,
                Status = ItemStatus.Equipped
            };

            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem1);
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem2);

            // Act
            var result = await _uniqueItemUseCase.GetPlayerUniqueItemsAsync(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => item.ItemName == "鉄の剣" && item.EnhancementLevel == 2);
            Assert.Contains(result, item => item.ItemName == "レザーアーマー" && item.IsEquipped == true);
        }

        [Fact]
        public async Task UseUniqueItem_ShouldToggleEquipStatus()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act - 装備する
            var result1 = await _uniqueItemUseCase.UseUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.True(result1.Success);
            Assert.Equal("鉄の剣を装備しました", result1.Message);

            // データベースで確認
            var updatedItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItem.UniqueItemId);
            Assert.NotNull(updatedItem);
            Assert.Equal(ItemStatus.Equipped, updatedItem.Status);

            // Act - 装備解除する
            var result2 = await _uniqueItemUseCase.UseUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.True(result2.Success);
            Assert.Equal("鉄の剣の装備を解除しました", result2.Message);

            // データベースで確認
            updatedItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItem.UniqueItemId);
            Assert.NotNull(updatedItem);
            Assert.Equal(ItemStatus.Inventory, updatedItem.Status);
        }

        [Fact]
        public async Task PurchaseUniqueItem_ShouldCreateNewItem()
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
            var result = await _uniqueItemUseCase.PurchaseUniqueItemAsync(1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("鉄の剣を購入しました", result.Message);
            Assert.Equal(500, result.TotalPrice);
            Assert.Equal(500, result.RemainingMoney);

            // アイテムが作成されていることを確認
            var uniqueItems = await _uniqueItemRepository.GetPlayerUniqueItemsAsync(1);
            Assert.Single(uniqueItems);
            Assert.Equal(1, uniqueItems[0].ItemMasterId);
            Assert.Equal(ItemStatus.Inventory, uniqueItems[0].Status);

            // プレイヤーの所持金が減っていることを確認
            var updatedPlayer = await _playerRepository.GetPlayerAsync(1);
            Assert.NotNull(updatedPlayer);
            Assert.Equal(500, updatedPlayer.Money);
        }

        [Fact]
        public async Task PurchaseUniqueItem_ShouldFail_WhenInsufficientMoney()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 100
            };
            await _playerRepository.CreatePlayerAsync(player);

            // Act
            var result = await _uniqueItemUseCase.PurchaseUniqueItemAsync(1, 1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("お金が不足しています", result.Message);
        }

        [Fact]
        public async Task SellUniqueItem_ShouldIncreaseMoneyAndMarkAsSold()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                EnhancementLevel = 2,
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act
            var result = await _uniqueItemUseCase.SellUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("鉄の剣を売却しました", result.Message);
            // 強化レベル2なので基本価格250の1.2倍 = 300
            Assert.Equal(300, result.TotalSellPrice);
            Assert.Equal(1300, result.RemainingMoney);

            // アイテムが売却済み状態になっていることを確認
            var soldItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItem.UniqueItemId);
            Assert.Null(soldItem); // 売却済みアイテムは取得できない

            // プレイヤーの所持金が増えていることを確認
            var updatedPlayer = await _playerRepository.GetPlayerAsync(1);
            Assert.NotNull(updatedPlayer);
            Assert.Equal(1300, updatedPlayer.Money);
        }

        [Fact]
        public async Task EnhanceUniqueItem_ShouldIncreaseLevel_WhenSuccessful()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 10000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                EnhancementLevel = 0,
                AttackBonus = 0,
                DefenseBonus = 0,
                HealthBonus = 0,
                ManaBonus = 0,
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act
            var result = await _uniqueItemUseCase.EnhanceUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("鉄の剣を+1に強化しました", result.Message);
            Assert.Equal(1, result.NewEnhancementLevel);
            Assert.Equal(1000, result.EnhancementCost); // 1000 * (0 + 1)
            Assert.Equal(9000, result.RemainingMoney);

            // データベースで確認
            var enhancedItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItem.UniqueItemId);
            Assert.NotNull(enhancedItem);
            Assert.Equal(1, enhancedItem.EnhancementLevel);
            // レアリティ2なので各ステータスに2*2=4ずつ加算
            Assert.Equal(4, enhancedItem.AttackBonus);
            Assert.Equal(4, enhancedItem.DefenseBonus);
            Assert.Equal(10, enhancedItem.HealthBonus); // 2*5
            Assert.Equal(6, enhancedItem.ManaBonus); // 2*3
        }

        [Fact]
        public async Task EnhanceUniqueItem_ShouldFail_WhenInsufficientMoney()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 500
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                EnhancementLevel = 0,
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act
            var result = await _uniqueItemUseCase.EnhanceUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("強化費用が不足しています", result.Message);
        }

        [Fact]
        public async Task EnhanceUniqueItem_ShouldFail_WhenMaxLevelReached()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 100000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 1,
                ItemMasterId = 1,
                EnhancementLevel = 10, // 最大レベル
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act
            var result = await _uniqueItemUseCase.EnhanceUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("これ以上強化できません", result.Message);
        }

        [Fact]
        public async Task UseUniqueItem_ShouldFail_WhenPlayerDoesNotOwnItem()
        {
            // Arrange
            var player1 = new PlayerEntity
            {
                UserId = 1,
                UserName = "Player1",
                Money = 1000
            };
            var player2 = new PlayerEntity
            {
                UserId = 2,
                UserName = "Player2",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player1);
            await _playerRepository.CreatePlayerAsync(player2);

            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = 2, // Player2が所有
                ItemMasterId = 1,
                Status = ItemStatus.Inventory
            };
            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // Act - Player1がPlayer2のアイテムを使用しようとする
            var result = await _uniqueItemUseCase.UseUniqueItemAsync(1, uniqueItem.UniqueItemId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("アイテムが存在しないか、所有者が異なります", result.Message);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}