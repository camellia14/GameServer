using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameServer.Entities
{
    /// <summary>
    /// プレイヤーを表すエンティティクラス
    /// プレイヤーの基本情報、所持金、インベントリを管理する
    /// </summary>
    public class PlayerEntity
    {
        /// <summary>
        /// プレイヤーの一意識別子
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// プレイヤーの所持金
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// プレイヤーのインベントリ（アイテムIDと所持数のマップ）
        /// </summary>
        private Dictionary<int, int> _inventory; // アイテムIDと所持数のマップ

        /// <summary>
        /// プレイヤーエンティティのコンストラクタ
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="userName">ユーザー名</param>
        public PlayerEntity(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            Money = 1000; // 初期所持金
            _inventory = new Dictionary<int, int>();
        }

        /// <summary>
        /// プレイヤーエンティティのデフォルトコンストラクタ
        /// </summary>
        public PlayerEntity()
        {
            UserName = string.Empty;
            Money = 1000;
            _inventory = new Dictionary<int, int>();
        }

        /// <summary>
        /// 指定した価格と数量のアイテムが購入可能かどうかを判定する
        /// </summary>
        /// <param name="price">アイテムの単価</param>
        /// <param name="amount">購入したい数量</param>
        /// <returns>購入可能な場合はtrue、そうでなければfalse</returns>
        public bool CanBuyItem(int price, int amount)
        {
            return Money >= price * amount;
        }

        /// <summary>
        /// プレイヤーの所持金を指定した金額分減らす
        /// </summary>
        /// <param name="amount">使用する金額</param>
        /// <exception cref="InvalidOperationException">所持金が不足している場合</exception>
        public void SpendMoney(int amount)
        {
            if (Money < amount)
            {
                throw new InvalidOperationException("所持金が不足しています。");
            }
            Money -= amount;
        }

        /// <summary>
        /// プレイヤーのインベントリにアイテムを追加する
        /// </summary>
        /// <param name="itemId">追加するアイテムのID</param>
        /// <param name="amount">追加する数量</param>
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

        /// <summary>
        /// プレイヤーが所持している指定アイテムの数量を取得する
        /// </summary>
        /// <param name="itemId">確認したいアイテムのID</param>
        /// <returns>所持している数量（所持していない場合は0）</returns>
        public int GetItemCount(int itemId)
        {
            return _inventory.ContainsKey(itemId) ? _inventory[itemId] : 0;
        }
    }
} 