using GameServer.MasterData;
using Xunit;

namespace UnitTests
{
    public class MasterDataTest
    {
        [Fact]
        public void ItemInfo_ShouldCreateCorrectly()
        {
            // Arrange & Act
            var itemInfo = new ItemInfo
            {
                Id = 1,
                Name = "テストアイテム",
                Description = "テスト用のアイテムです",
                ItemType = ItemType.Stack,
                Price = 100,
                SellPrice = 50,
                MaxStackCount = 99,
                EffectScript = "test_effect",
                Rarity = 3
            };

            // Assert
            Assert.Equal(1, itemInfo.Id);
            Assert.Equal("テストアイテム", itemInfo.Name);
            Assert.Equal("テスト用のアイテムです", itemInfo.Description);
            Assert.Equal(ItemType.Stack, itemInfo.ItemType);
            Assert.Equal(100, itemInfo.Price);
            Assert.Equal(50, itemInfo.SellPrice);
            Assert.Equal(99, itemInfo.MaxStackCount);
            Assert.Equal("test_effect", itemInfo.EffectScript);
            Assert.Equal(3, itemInfo.Rarity);
        }

        [Fact]
        public void ItemMaster_ShouldLoadDataCorrectly()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "アイテム1", ItemType = ItemType.Stack, Price = 100, SellPrice = 50, Rarity = 1 },
                new ItemInfo { Id = 2, Name = "アイテム2", ItemType = ItemType.Unique, Price = 500, SellPrice = 250, Rarity = 3 },
                new ItemInfo { Id = 3, Name = "アイテム3", ItemType = ItemType.Stack, Price = 0, SellPrice = 0, Rarity = 2 }
            };

            // Act
            itemMaster.LoadData(testData);

            // Assert
            Assert.Equal(3, itemMaster.Count);
            Assert.NotNull(itemMaster.GetById(1));
            Assert.NotNull(itemMaster.GetById(2));
            Assert.NotNull(itemMaster.GetById(3));
            Assert.Null(itemMaster.GetById(4));
        }

        [Fact]
        public void ItemMaster_GetByType_ShouldReturnCorrectItems()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "スタックアイテム1", ItemType = ItemType.Stack },
                new ItemInfo { Id = 2, Name = "ユニークアイテム1", ItemType = ItemType.Unique },
                new ItemInfo { Id = 3, Name = "スタックアイテム2", ItemType = ItemType.Stack },
                new ItemInfo { Id = 4, Name = "ユニークアイテム2", ItemType = ItemType.Unique }
            };
            itemMaster.LoadData(testData);

            // Act
            var stackItems = itemMaster.GetStackItems();
            var uniqueItems = itemMaster.GetUniqueItems();

            // Assert
            Assert.Equal(2, stackItems.Count);
            Assert.Equal(2, uniqueItems.Count);
            Assert.Contains(stackItems, item => item.Name == "スタックアイテム1");
            Assert.Contains(stackItems, item => item.Name == "スタックアイテム2");
            Assert.Contains(uniqueItems, item => item.Name == "ユニークアイテム1");
            Assert.Contains(uniqueItems, item => item.Name == "ユニークアイテム2");
        }

        [Fact]
        public void ItemMaster_GetByRarity_ShouldReturnCorrectItems()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "コモンアイテム", Rarity = 1 },
                new ItemInfo { Id = 2, Name = "レアアイテム", Rarity = 3 },
                new ItemInfo { Id = 3, Name = "レジェンダリーアイテム", Rarity = 5 },
                new ItemInfo { Id = 4, Name = "コモンアイテム2", Rarity = 1 }
            };
            itemMaster.LoadData(testData);

            // Act
            var commonItems = itemMaster.GetByRarity(1);
            var rareItems = itemMaster.GetByRarity(3);
            var legendaryItems = itemMaster.GetByRarity(5);

            // Assert
            Assert.Equal(2, commonItems.Count);
            Assert.Single(rareItems);
            Assert.Single(legendaryItems);
            Assert.Equal("レアアイテム", rareItems.First().Name);
            Assert.Equal("レジェンダリーアイテム", legendaryItems.First().Name);
        }

        [Fact]
        public void ItemMaster_GetPurchasableItems_ShouldReturnOnlyPurchasableItems()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "購入可能アイテム1", Price = 100 },
                new ItemInfo { Id = 2, Name = "購入不可アイテム", Price = 0 },
                new ItemInfo { Id = 3, Name = "購入可能アイテム2", Price = 500 }
            };
            itemMaster.LoadData(testData);

            // Act
            var purchasableItems = itemMaster.GetPurchasableItems();

            // Assert
            Assert.Equal(2, purchasableItems.Count);
            Assert.Contains(purchasableItems, item => item.Name == "購入可能アイテム1");
            Assert.Contains(purchasableItems, item => item.Name == "購入可能アイテム2");
            Assert.DoesNotContain(purchasableItems, item => item.Name == "購入不可アイテム");
        }

        [Fact]
        public void ItemMaster_GetSellableItems_ShouldReturnOnlySellableItems()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "売却可能アイテム1", SellPrice = 50 },
                new ItemInfo { Id = 2, Name = "売却不可アイテム", SellPrice = 0 },
                new ItemInfo { Id = 3, Name = "売却可能アイテム2", SellPrice = 250 }
            };
            itemMaster.LoadData(testData);

            // Act
            var sellableItems = itemMaster.GetSellableItems();

            // Assert
            Assert.Equal(2, sellableItems.Count);
            Assert.Contains(sellableItems, item => item.Name == "売却可能アイテム1");
            Assert.Contains(sellableItems, item => item.Name == "売却可能アイテム2");
            Assert.DoesNotContain(sellableItems, item => item.Name == "売却不可アイテム");
        }

        [Fact]
        public void ItemMaster_Exists_ShouldReturnCorrectResults()
        {
            // Arrange
            var itemMaster = new ItemMaster();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "アイテム1" },
                new ItemInfo { Id = 2, Name = "アイテム2" }
            };
            itemMaster.LoadData(testData);

            // Act & Assert
            Assert.True(itemMaster.Exists(1));
            Assert.True(itemMaster.Exists(2));
            Assert.False(itemMaster.Exists(3));
            Assert.False(itemMaster.Exists(999));
        }

        [Fact]
        public void MasterDataManager_ShouldBeSingleton()
        {
            // Act
            var instance1 = MasterDataManager.Instance;
            var instance2 = MasterDataManager.Instance;

            // Assert
            Assert.Same(instance1, instance2);
            Assert.NotNull(instance1.ItemMaster);
        }

        [Fact]
        public void MasterDataManager_IsInitialized_ShouldReturnCorrectStatus()
        {
            // Arrange
            var manager = MasterDataManager.Instance;
            manager.Reset(); // テスト用にリセット

            // Act & Assert - 初期状態では未初期化
            Assert.False(manager.IsInitialized());

            // データを読み込み後は初期化済み
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "テストアイテム" }
            };
            manager.ItemMaster.LoadData(testData);
            Assert.True(manager.IsInitialized());
        }

        [Fact]
        public void MasterDataManager_GetStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange
            var manager = MasterDataManager.Instance;
            manager.Reset();
            var testData = new List<ItemInfo>
            {
                new ItemInfo { Id = 1, Name = "スタックアイテム1", ItemType = ItemType.Stack, Price = 100, SellPrice = 50 },
                new ItemInfo { Id = 2, Name = "ユニークアイテム1", ItemType = ItemType.Unique, Price = 500, SellPrice = 250 },
                new ItemInfo { Id = 3, Name = "スタックアイテム2", ItemType = ItemType.Stack, Price = 0, SellPrice = 0 },
                new ItemInfo { Id = 4, Name = "ユニークアイテム2", ItemType = ItemType.Unique, Price = 0, SellPrice = 100 }
            };
            manager.ItemMaster.LoadData(testData);

            // Act
            var statistics = manager.GetStatistics();

            // Assert
            Assert.Equal(4, statistics["TotalItems"]);
            Assert.Equal(2, statistics["StackItems"]);
            Assert.Equal(2, statistics["UniqueItems"]);
            Assert.Equal(2, statistics["PurchasableItems"]);
            Assert.Equal(3, statistics["SellableItems"]);
        }
    }
}