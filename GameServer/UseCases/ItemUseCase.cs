using System;
using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;

namespace GameServer.UseCases
{
    public class ItemUseCase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPlayerRepository _playerRepository;

        public ItemUseCase(IItemRepository itemRepository, IPlayerRepository playerRepository)
        {
            _itemRepository = itemRepository;
            _playerRepository = playerRepository;
        }

        public async Task<bool> BuyItem(int userId, int itemId, int amount)
        {
            // プレイヤーとアイテムの取得
            var player = await _playerRepository.GetByIdAsync(userId);
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (player == null || item == null)
            {
                return false;
            }

            // 購入可能かチェック
            if (!player.CanBuyItem(item.Price, amount) || !item.CanBuy(amount))
            {
                return false;
            }

            try
            {
                // 購入処理
                player.SpendMoney(item.Price * amount);
                item.DecreaseStock(amount);
                player.AddItem(itemId, amount);

                // 変更を保存
                await _playerRepository.UpdateAsync(player);
                await _itemRepository.UpdateAsync(item);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 