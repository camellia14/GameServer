using System;
using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;

namespace GameServer.UseCases
{
    public class PlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerUseCase(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public async Task<PlayerEntity?> GetPlayerByIdAsync(int userId)
        {
            // プレイヤーをIDで取得
            var player = await _playerRepository.GetByIdAsync(userId);
            return player;
        }
    }
} 