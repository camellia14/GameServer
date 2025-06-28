using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    public interface IPlayerService : IService<IPlayerService>
    {
        UnaryResult<PlayerData?> GetPlayer(int id);
        UnaryResult<PlayerData> CreatePlayer(string userName);
    }
}
