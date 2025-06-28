using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories;
using GameServer.UseCases;
using GameServer.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MagicOnion.Server;
using Grpc.Core;

namespace UnitTests
{
    public class CharacterServiceTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CharacterRepository _characterRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly CharacterUseCase _characterUseCase;
        private readonly CharacterService _characterService;

        public CharacterServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _characterRepository = new CharacterRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _characterUseCase = new CharacterUseCase(_characterRepository, _playerRepository);
            _characterService = new CharacterService(_characterUseCase);
        }

        [Fact]
        public async Task GetMyCharacters_ShouldReturnCurrentPlayerCharacters_WithDefaultPlayerId()
        {
            // このテストはCharacterUseCaseの機能をテストする
            // CharacterServiceのGetMyCharactersは内部でGetPlayerCharactersAsyncを呼ぶ
            
            // Arrange - デフォルトプレイヤー（ID=1）を作成
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "DefaultPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            // キャラクターを作成
            await _characterUseCase.CreateCharacterAsync(player.UserId, "Character1");
            await _characterUseCase.CreateCharacterAsync(player.UserId, "Character2");

            // 他のプレイヤーのキャラクターも作成（混入テスト）
            var otherPlayer = new PlayerEntity
            {
                UserId = 2,
                UserName = "OtherPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(otherPlayer);
            await _characterUseCase.CreateCharacterAsync(otherPlayer.UserId, "OtherCharacter");

            // Act - CharacterUseCaseを直接使ってテスト（デフォルトplayerId=1）
            var result = await _characterUseCase.GetPlayerCharactersAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Character1");
            Assert.Contains(result, c => c.Name == "Character2");
            Assert.DoesNotContain(result, c => c.Name == "OtherCharacter");
        }

        [Fact]
        public async Task GetMyCharacters_ShouldReturnEmptyList_WhenPlayerHasNoCharacters()
        {
            // Arrange - プレイヤーを作成するがキャラクターは作成しない
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "PlayerWithoutCharacters",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            // Act - CharacterUseCaseを直接使ってテスト
            var result = await _characterUseCase.GetPlayerCharactersAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMyCharacters_ShouldHandleErrorGracefully_WhenPlayerDoesNotExist()
        {
            // Arrange - プレイヤーを作成しない（存在しないプレイヤー）

            // Act - 存在しないプレイヤーIDでテスト
            var result = await _characterUseCase.GetPlayerCharactersAsync(999);

            // Assert - エラーが発生してもnullではなく空のリストを返すことを確認
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMyCharacters_ShouldReturnCharactersInCorrectOrder()
        {
            // Arrange
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            // 複数のキャラクターを作成（作成順序をテスト）
            var char1 = await _characterUseCase.CreateCharacterAsync(player.UserId, "Alpha");
            await Task.Delay(1);
            var char2 = await _characterUseCase.CreateCharacterAsync(player.UserId, "Beta");
            await Task.Delay(1);
            var char3 = await _characterUseCase.CreateCharacterAsync(player.UserId, "Gamma");

            // Act - CharacterUseCaseを直接使ってテスト
            var result = await _characterUseCase.GetPlayerCharactersAsync(1);

            // Assert - 作成順序で返されることを確認
            Assert.Equal(3, result.Count);
            Assert.Equal("Alpha", result[0].Name);
            Assert.Equal("Beta", result[1].Name);
            Assert.Equal("Gamma", result[2].Name);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}