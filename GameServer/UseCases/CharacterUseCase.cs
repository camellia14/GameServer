using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Shared.Data;

namespace GameServer.UseCases
{
    public class CharacterUseCase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IPlayerRepository _playerRepository;

        public CharacterUseCase(ICharacterRepository characterRepository, IPlayerRepository playerRepository)
        {
            _characterRepository = characterRepository;
            _playerRepository = playerRepository;
        }

        public async Task<CharacterData?> GetCharacterAsync(int characterId)
        {
            var character = await _characterRepository.GetCharacterByIdAsync(characterId);
            if (character == null)
                return null;

            return MapToCharacterData(character);
        }

        public async Task<List<CharacterData>> GetPlayerCharactersAsync(int playerUserId)
        {
            var characters = await _characterRepository.GetCharactersByPlayerIdAsync(playerUserId);
            return characters.Select(MapToCharacterData).ToList();
        }

        public async Task<CharacterData?> CreateCharacterAsync(int playerUserId, string characterName)
        {
            if (string.IsNullOrWhiteSpace(characterName))
                throw new ArgumentException("Character name cannot be empty");

            if (characterName.Length > 50)
                throw new ArgumentException("Character name must be 50 characters or less");

            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            var character = new CharacterEntity
            {
                PlayerUserId = playerUserId,
                Name = characterName.Trim(),
                Level = 1,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            var createdCharacter = await _characterRepository.CreateCharacterAsync(character);
            return MapToCharacterData(createdCharacter);
        }

        public async Task<CharacterData?> MoveCharacterAsync(int characterId, float positionX, float positionY)
        {
            var character = await _characterRepository.GetCharacterByIdAsync(characterId);
            if (character == null)
                return null;

            character.PositionX = positionX;
            character.PositionY = positionY;

            var updatedCharacter = await _characterRepository.UpdateCharacterAsync(character);
            return MapToCharacterData(updatedCharacter);
        }

        public async Task DeleteCharacterAsync(int characterId)
        {
            var exists = await _characterRepository.CharacterExistsAsync(characterId);
            if (!exists)
                throw new InvalidOperationException("Character not found");

            await _characterRepository.DeleteCharacterAsync(characterId);
        }

        private static CharacterData MapToCharacterData(CharacterEntity character)
        {
            return new CharacterData
            {
                CharacterId = character.CharacterId,
                PlayerUserId = character.PlayerUserId,
                Name = character.Name,
                Level = character.Level,
                PositionX = character.PositionX,
                PositionY = character.PositionY,
                CreatedAt = character.CreatedAt,
                UpdatedAt = character.UpdatedAt
            };
        }
    }
}