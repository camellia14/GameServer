using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories;
using GameServer.UseCases;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class ItemTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ItemRepository _itemRepository;
        private readonly ItemUseCase _itemUseCase;
        private readonly PlayerRepository _playerRepository;

        public ItemTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _itemRepository = new ItemRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _itemUseCase = new ItemUseCase(_itemRepository, _playerRepository);
        }

        [Fact]
        public async Task CreateItem_ShouldCreateItemSuccessfully()
        {
            var item = new ItemEntity
            {
                Name = "Test Item",
                Description = "Test Description",
                Price = 100,
                Stock = 10
            };

            var createdItem = await _itemRepository.CreateAsync(item);

            Assert.NotEqual(0, createdItem.Id);
            Assert.Equal("Test Item", createdItem.Name);
            Assert.Equal(100, createdItem.Price);
            Assert.Equal(10, createdItem.Stock);
        }

        [Fact]
        public async Task GetItem_ShouldReturnItem_WhenItemExists()
        {
            var item = new ItemEntity
            {
                Name = "Get Test Item",
                Description = "Get Test Description",
                Price = 200,
                Stock = 5
            };

            var createdItem = await _itemRepository.CreateAsync(item);
            var retrievedItem = await _itemRepository.GetByIdAsync(createdItem.Id);

            Assert.NotNull(retrievedItem);
            Assert.Equal(createdItem.Id, retrievedItem.Id);
            Assert.Equal("Get Test Item", retrievedItem.Name);
        }

        [Fact]
        public async Task BuyItem_ShouldSucceed_WhenPlayerHasEnoughMoney()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var item = new ItemEntity
            {
                Name = "Buyable Item",
                Description = "Can be bought",
                Price = 500,
                Stock = 1
            };
            var createdItem = await _itemRepository.CreateAsync(item);

            var result = await _itemUseCase.BuyItem(player.UserId, createdItem.Id, 1);

            Assert.True(result);
            
            var updatedPlayer = await _playerRepository.GetPlayerAsync(player.UserId);
            Assert.Equal(500, updatedPlayer?.Money);
            
            var updatedItem = await _itemRepository.GetByIdAsync(createdItem.Id);
            Assert.Equal(0, updatedItem?.Stock);
        }

        [Fact]
        public async Task BuyItem_ShouldFail_WhenPlayerHasInsufficientMoney()
        {
            var player = new PlayerEntity
            {
                UserId = 2,
                UserName = "PoorPlayer",
                Money = 100
            };
            await _playerRepository.CreatePlayerAsync(player);

            var item = new ItemEntity
            {
                Name = "Expensive Item",
                Description = "Too expensive",
                Price = 500,
                Stock = 1
            };
            var createdItem = await _itemRepository.CreateAsync(item);

            var result = await _itemUseCase.BuyItem(player.UserId, createdItem.Id, 1);

            Assert.False(result);
            
            var updatedPlayer = await _playerRepository.GetPlayerAsync(player.UserId);
            Assert.Equal(100, updatedPlayer?.Money);
            
            var updatedItem = await _itemRepository.GetByIdAsync(createdItem.Id);
            Assert.Equal(1, updatedItem?.Stock);
        }

        [Fact]
        public async Task BuyItem_ShouldFail_WhenItemOutOfStock()
        {
            var player = new PlayerEntity
            {
                UserId = 3,
                UserName = "RichPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var item = new ItemEntity
            {
                Name = "Out of Stock Item",
                Description = "No stock",
                Price = 100,
                Stock = 0
            };
            var createdItem = await _itemRepository.CreateAsync(item);

            var result = await _itemUseCase.BuyItem(player.UserId, createdItem.Id, 1);

            Assert.False(result);
            
            var updatedPlayer = await _playerRepository.GetPlayerAsync(player.UserId);
            Assert.Equal(1000, updatedPlayer?.Money);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
