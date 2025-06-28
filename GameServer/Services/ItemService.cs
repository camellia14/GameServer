using MagicOnion;
using MagicOnion.Server;
using GameServer.Repositories.Interfaces;
using Shared.Services;
using Shared.Data;

namespace GameServer.Services
{
    public class ItemService : ServiceBase<IItemService>, IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async UnaryResult<ItemData?> GetItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new ItemData
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Stock = item.Stock
            };
        }

        // 他のメソッドも同様に追加可能
    }
}
