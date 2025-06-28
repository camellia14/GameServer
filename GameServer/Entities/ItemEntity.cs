using System;
using System.ComponentModel.DataAnnotations;

namespace GameServer.Entities
{
    /// <summary>
    /// ゲーム内アイテムを表すエンティティクラス
    /// アイテムの購入や在庫管理を行う
    /// </summary>
    public class ItemEntity
    {
        /// <summary>
        /// アイテムの一意識別子
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// アイテムの説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// アイテムの価格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// アイテムの在庫数
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// アイテムの作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// アイテムの最終更新日時
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 指定した数量のアイテムが購入可能かどうかを判定する
        /// </summary>
        /// <param name="amount">購入したい数量</param>
        /// <returns>購入可能な場合はtrue、そうでなければfalse</returns>
        public bool CanBuy(int amount)
        {
            return Stock >= amount;
        }

        /// <summary>
        /// アイテムの在庫を指定した数量分減らす
        /// </summary>
        /// <param name="amount">減らしたい数量</param>
        /// <exception cref="InvalidOperationException">在庫が不足している場合</exception>
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