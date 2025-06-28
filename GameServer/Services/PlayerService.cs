using MagicOnion;
using MagicOnion.Server;
using GameServer.Repositories.Interfaces;
using Shared.Services;
using Shared.Data;
using GameServer.Entities;

namespace GameServer.Services
{
    public class PlayerService : ServiceBase<IPlayerService>, IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public async UnaryResult<PlayerData?> GetPlayer(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null)
            {
                return new PlayerData
                {
                    UserId = 0,
                    UserName = "",
                    Money = 0
                };
            }
            return new PlayerData
            {
                UserId = player.UserId,
                UserName = player.UserName,
                Money = player.Money
            };
        }
        public async UnaryResult<PlayerData> CreatePlayer(string userName)
        {
            var playerEntity = new PlayerEntity(0, userName);
            await _playerRepository.Create(playerEntity);
            PlayerData playerData = new PlayerData
            {
                UserId = 0,
                UserName = userName,
                Money = 0
            };
            return playerData;
        }
    }
}

