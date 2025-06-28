using System;
using System.ComponentModel.DataAnnotations;

namespace GameServer.Entities
{
    public class ItemEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool CanBuy(int amount)
        {
            return Stock >= amount;
        }

        public void DecreaseStock(int amount)
        {
            if (!CanBuy(amount))
            {
                throw new InvalidOperationException("在庫が不足しています。");
            }
            Stock -= amount;
        }
    }
} 