using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    public interface ICharacterService : IService<ICharacterService>
    {
        UnaryResult<CharacterData?> GetCharacter(int characterId);
        UnaryResult<List<CharacterData>> GetPlayerCharacters(int playerUserId);
        UnaryResult<CharacterData?> CreateCharacter(int playerUserId, string characterName);
        UnaryResult<CharacterData?> MoveCharacter(int characterId, float positionX, float positionY);
        UnaryResult<bool> DeleteCharacter(int characterId);
    }
}