using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemEntity?> GetByIdAsync(int itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }

        public async Task<List<ItemEntity>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<ItemEntity> CreateAsync(ItemEntity item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(ItemEntity item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int itemId)
        {
            return await _context.Items.AnyAsync(i => i.Id == itemId);
        }

        public async Task<List<ItemEntity>> GetByNameAsync(string name)
        {
            return await _context.Items
                .Where(i => i.Name.Contains(name))
                .ToListAsync();
        }
    }
} 