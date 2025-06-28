using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    public interface ICharacterRepository
    {
        Task<CharacterEntity?> GetCharacterByIdAsync(int characterId);
        Task<List<CharacterEntity>> GetCharactersByPlayerIdAsync(int playerUserId);
        Task<CharacterEntity> CreateCharacterAsync(CharacterEntity character);
        Task<CharacterEntity> UpdateCharacterAsync(CharacterEntity character);
        Task DeleteCharacterAsync(int characterId);
        Task<bool> CharacterExistsAsync(int characterId);
        Task<bool> IsCharacterOwnedByPlayerAsync(int characterId, int playerUserId);
    }
}