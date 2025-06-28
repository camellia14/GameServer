using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    public interface IItemService : IService<IItemService>
    {
        UnaryResult<ItemData?> GetItem(int id);
    }
}
