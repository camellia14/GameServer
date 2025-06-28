using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories;
using GameServer.UseCases;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class CharacterTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CharacterRepository _characterRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly CharacterUseCase _characterUseCase;

        public CharacterTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _characterRepository = new CharacterRepository(_context);
            _playerRepository = new PlayerRepository(_context);
            _characterUseCase = new CharacterUseCase(_characterRepository, _playerRepository);
        }

        [Fact]
        public async Task CreateCharacter_ShouldCreateCharacterSuccessfully()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var characterData = await _characterUseCase.CreateCharacterAsync(player.UserId, "TestCharacter");

            Assert.NotNull(characterData);
            Assert.Equal("TestCharacter", characterData.Name);
            Assert.Equal(player.UserId, characterData.PlayerUserId);
            Assert.Equal(1, characterData.Level);
            Assert.Equal(0.0f, characterData.PositionX);
            Assert.Equal(0.0f, characterData.PositionY);
        }

        [Fact]
        public async Task CreateCharacter_ShouldThrowException_WhenPlayerNotFound()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _characterUseCase.CreateCharacterAsync(999, "TestCharacter");
            });
        }

        [Fact]
        public async Task CreateCharacter_ShouldThrowException_WhenNameIsEmpty()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _characterUseCase.CreateCharacterAsync(player.UserId, "");
            });
        }

        [Fact]
        public async Task CreateCharacter_ShouldThrowException_WhenNameIsTooLong()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var longName = new string('A', 51);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _characterUseCase.CreateCharacterAsync(player.UserId, longName);
            });
        }

        [Fact]
        public async Task GetCharacter_ShouldReturnCharacter_WhenCharacterExists()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var createdCharacter = await _characterUseCase.CreateCharacterAsync(player.UserId, "GetTestCharacter");
            var retrievedCharacter = await _characterUseCase.GetCharacterAsync(createdCharacter!.CharacterId);

            Assert.NotNull(retrievedCharacter);
            Assert.Equal(createdCharacter.CharacterId, retrievedCharacter.CharacterId);
            Assert.Equal("GetTestCharacter", retrievedCharacter.Name);
        }

        [Fact]
        public async Task GetPlayerCharacters_ShouldReturnAllPlayerCharacters()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            await _characterUseCase.CreateCharacterAsync(player.UserId, "Character1");
            await _characterUseCase.CreateCharacterAsync(player.UserId, "Character2");
            await _characterUseCase.CreateCharacterAsync(player.UserId, "Character3");

            var characters = await _characterUseCase.GetPlayerCharactersAsync(player.UserId);

            Assert.Equal(3, characters.Count);
            Assert.Contains(characters, c => c.Name == "Character1");
            Assert.Contains(characters, c => c.Name == "Character2");
            Assert.Contains(characters, c => c.Name == "Character3");
        }

        [Fact]
        public async Task MoveCharacter_ShouldUpdatePosition()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var character = await _characterUseCase.CreateCharacterAsync(player.UserId, "MovableCharacter");
            var movedCharacter = await _characterUseCase.MoveCharacterAsync(character!.CharacterId, 10.5f, 20.3f);

            Assert.NotNull(movedCharacter);
            Assert.Equal(10.5f, movedCharacter.PositionX);
            Assert.Equal(20.3f, movedCharacter.PositionY);
        }

        [Fact]
        public async Task DeleteCharacter_ShouldRemoveCharacter()
        {
            var player = new PlayerEntity
            {
                UserId = 1,
                UserName = "TestPlayer",
                Money = 1000
            };
            await _playerRepository.CreatePlayerAsync(player);

            var character = await _characterUseCase.CreateCharacterAsync(player.UserId, "DeleteTestCharacter");
            await _characterUseCase.DeleteCharacterAsync(character!.CharacterId);

            var deletedCharacter = await _characterUseCase.GetCharacterAsync(character.CharacterId);
            Assert.Null(deletedCharacter);
        }

        [Fact]
        public async Task DeleteCharacter_ShouldThrowException_WhenCharacterNotFound()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _characterUseCase.DeleteCharacterAsync(999);
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}