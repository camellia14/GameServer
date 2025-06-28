using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameServer.Entities
{
    public class PlayerEntity
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Money { get; set; }
        private Dictionary<int, int> _inventory; // アイテムIDと所持数のマップ

        public PlayerEntity(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            Money = 1000; // 初期所持金
            _inventory = new Dictionary<int, int>();
        }

        public PlayerEntity()
        {
            UserName = string.Empty;
            Money = 1000;
            _inventory = new Dictionary<int, int>();
        }

        public bool CanBuyItem(int price, int amount)
        {
            return Money >= price * amount;
        }

        public void SpendMoney(int amount)
        {
            if (Money < amount)
            {
                throw new InvalidOperationException("所持金が不足しています。");
            }
            Money -= amount;
        }

        public void AddItem(int itemId, int amount)
        {
            if (_inventory.ContainsKey(itemId))
            {
                _inventory[itemId] += amount;
            }
            else
            {
                _inventory[itemId] = amount;
            }
        }

        public int GetItemCount(int itemId)
        {
            return _inventory.ContainsKey(itemId) ? _inventory[itemId] : 0;
        }
    }
} 