using MagicOnion;
using MagicOnion.Server;
using Shared.Services;
using Shared.Data;
using GameServer.UseCases;

namespace GameServer.Services
{
    public class CharacterService : ServiceBase<ICharacterService>, ICharacterService
    {
        private readonly CharacterUseCase _characterUseCase;

        public CharacterService(CharacterUseCase characterUseCase)
        {
            _characterUseCase = characterUseCase;
        }

        public async UnaryResult<CharacterData?> GetCharacter(int characterId)
        {
            try
            {
                return await _characterUseCase.GetCharacterAsync(characterId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCharacter: {ex.Message}");
                return null;
            }
        }

        public async UnaryResult<List<CharacterData>> GetPlayerCharacters(int playerUserId)
        {
            try
            {
                return await _characterUseCase.GetPlayerCharactersAsync(playerUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPlayerCharacters: {ex.Message}");
                return new List<CharacterData>();
            }
        }

        public async UnaryResult<CharacterData?> CreateCharacter(int playerUserId, string characterName)
        {
            try
            {
                return await _characterUseCase.CreateCharacterAsync(playerUserId, characterName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCharacter: {ex.Message}");
                return null;
            }
        }

        public async UnaryResult<CharacterData?> MoveCharacter(int characterId, float positionX, float positionY)
        {
            try
            {
                return await _characterUseCase.MoveCharacterAsync(characterId, positionX, positionY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MoveCharacter: {ex.Message}");
                return null;
            }
        }

        public async UnaryResult<bool> DeleteCharacter(int characterId)
        {
            try
            {
                await _characterUseCase.DeleteCharacterAsync(characterId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCharacter: {ex.Message}");
                return false;
            }
        }
    }
}