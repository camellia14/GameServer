using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CharacterEntity?> GetCharacterByIdAsync(int characterId)
        {
            return await _context.Characters
                .Include(c => c.Player)
                .FirstOrDefaultAsync(c => c.CharacterId == characterId);
        }

        public async Task<List<CharacterEntity>> GetCharactersByPlayerIdAsync(int playerUserId)
        {
            return await _context.Characters
                .Where(c => c.PlayerUserId == playerUserId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<CharacterEntity> CreateCharacterAsync(CharacterEntity character)
        {
            character.CreatedAt = DateTime.UtcNow;
            character.UpdatedAt = DateTime.UtcNow;
            
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<CharacterEntity> UpdateCharacterAsync(CharacterEntity character)
        {
            character.UpdatedAt = DateTime.UtcNow;
            
            _context.Characters.Update(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task DeleteCharacterAsync(int characterId)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CharacterExistsAsync(int characterId)
        {
            return await _context.Characters.AnyAsync(c => c.CharacterId == characterId);
        }

        public async Task<bool> IsCharacterOwnedByPlayerAsync(int characterId, int playerUserId)
        {
            return await _context.Characters.AnyAsync(c => c.CharacterId == characterId && c.PlayerUserId == playerUserId);
        }
    }
}