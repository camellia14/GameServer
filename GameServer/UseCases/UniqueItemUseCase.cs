using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories.Interfaces;
using Shared.Data;

namespace GameServer.UseCases
{
    /// <summary>
    /// ユニークアイテムに関するビジネスロジックを提供するユースケースクラス
    /// </summary>
    public class UniqueItemUseCase
    {
        private readonly IUniqueItemRepository _uniqueItemRepository;
        private readonly IPlayerRepository _playerRepository;

        /// <summary>
        /// UniqueItemUseCaseのコンストラクタ
        /// </summary>
        /// <param name="uniqueItemRepository">ユニークアイテムリポジトリ</param>
        /// <param name="playerRepository">プレイヤーリポジトリ</param>
        public UniqueItemUseCase(IUniqueItemRepository uniqueItemRepository, IPlayerRepository playerRepository)
        {
            _uniqueItemRepository = uniqueItemRepository;
            _playerRepository = playerRepository;
        }

        /// <summary>
        /// プレイヤーが所有するユニークアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>ユニークアイテムデータのリスト</returns>
        public async Task<List<UniqueItemData>> GetPlayerUniqueItemsAsync(int playerUserId)
        {
            var uniqueItems = await _uniqueItemRepository.GetPlayerUniqueItemsAsync(playerUserId);
            var itemMaster = MasterDataManager.Instance.ItemMaster;

            return uniqueItems.Select(item =>
            {
                var itemInfo = itemMaster.GetById(item.ItemMasterId);
                return new UniqueItemData
                {
                    UniqueItemId = item.UniqueItemId,
                    ItemMasterId = item.ItemMasterId,
                    ItemName = itemInfo?.Name ?? "Unknown Item",
                    EnhancementLevel = item.EnhancementLevel,
                    AttackBonus = item.AttackBonus,
                    DefenseBonus = item.DefenseBonus,
                    HealthBonus = item.HealthBonus,
                    ManaBonus = item.ManaBonus,
                    Status = item.Status.ToString(),
                    IsEquipped = item.Status == ItemStatus.Equipped
                };
            }).ToList();
        }

        /// <summary>
        /// ユニークアイテムを使用する（装備/装備解除）
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="uniqueItemId">使用するユニークアイテムのID</param>
        /// <returns>使用結果</returns>
        public async Task<ItemUseResult> UseUniqueItemAsync(int playerUserId, int uniqueItemId)
        {
            // プレイヤーの存在確認
            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "プレイヤーが存在しません"
                };
            }

            // ユニークアイテムの確認
            var uniqueItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItemId);
            if (uniqueItem == null || uniqueItem.PlayerUserId != playerUserId)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテムが存在しないか、所有者が異なります"
                };
            }

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(uniqueItem.ItemMasterId);
            if (itemInfo == null)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテム情報が見つかりません"
                };
            }

            // 装備状態を切り替え
            string message;
            if (uniqueItem.Status == ItemStatus.Equipped)
            {
                uniqueItem.Status = ItemStatus.Inventory;
                message = $"{itemInfo.Name}の装備を解除しました";
            }
            else if (uniqueItem.Status == ItemStatus.Inventory)
            {
                uniqueItem.Status = ItemStatus.Equipped;
                message = $"{itemInfo.Name}を装備しました";
            }
            else
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "このアイテムは装備できません"
                };
            }

            await _uniqueItemRepository.UpdateUniqueItemAsync(uniqueItem);

            return new ItemUseResult
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// ユニークアイテムを購入する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">購入するアイテムのマスターID</param>
        /// <returns>購入結果</returns>
        public async Task<ItemPurchaseResult> PurchaseUniqueItemAsync(int playerUserId, int itemMasterId)
        {
            // プレイヤーの存在確認
            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "プレイヤーが存在しません"
                };
            }

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(itemMasterId);
            if (itemInfo == null)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "アイテムが存在しません"
                };
            }

            if (itemInfo.ItemType != ItemType.Unique)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "このアイテムはユニークアイテムではありません"
                };
            }

            if (itemInfo.Price <= 0)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "このアイテムは購入できません"
                };
            }

            if (player.Money < itemInfo.Price)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "お金が不足しています"
                };
            }

            // ユニークアイテムを作成
            var uniqueItem = new UniqueItemEntity
            {
                PlayerUserId = playerUserId,
                ItemMasterId = itemMasterId,
                EnhancementLevel = 0,
                AttackBonus = 0,
                DefenseBonus = 0,
                HealthBonus = 0,
                ManaBonus = 0,
                Status = ItemStatus.Inventory
            };

            await _uniqueItemRepository.CreateUniqueItemAsync(uniqueItem);

            // プレイヤーの所持金を減らす
            player.Money -= itemInfo.Price;
            await _playerRepository.UpdatePlayerAsync(player);

            return new ItemPurchaseResult
            {
                Success = true,
                Message = $"{itemInfo.Name}を購入しました",
                TotalPrice = itemInfo.Price,
                RemainingMoney = player.Money
            };
        }

        /// <summary>
        /// ユニークアイテムを売却する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="uniqueItemId">売却するユニークアイテムのID</param>
        /// <returns>売却結果</returns>
        public async Task<ItemSellResult> SellUniqueItemAsync(int playerUserId, int uniqueItemId)
        {
            // プレイヤーの存在確認
            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "プレイヤーが存在しません"
                };
            }

            // ユニークアイテムの確認
            var uniqueItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItemId);
            if (uniqueItem == null || uniqueItem.PlayerUserId != playerUserId)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテムが存在しないか、所有者が異なります"
                };
            }

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(uniqueItem.ItemMasterId);
            if (itemInfo == null)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテム情報が見つかりません"
                };
            }

            if (itemInfo.SellPrice <= 0)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "このアイテムは売却できません"
                };
            }

            // 強化レベルに応じて売却価格を調整
            var enhancementMultiplier = 1.0f + (uniqueItem.EnhancementLevel * 0.1f);
            var totalSellPrice = (int)(itemInfo.SellPrice * enhancementMultiplier);

            // アイテムを削除（売却済み状態に変更）
            await _uniqueItemRepository.DeleteUniqueItemAsync(uniqueItemId);

            // プレイヤーの所持金を増やす
            player.Money += totalSellPrice;
            await _playerRepository.UpdatePlayerAsync(player);

            return new ItemSellResult
            {
                Success = true,
                Message = $"{itemInfo.Name}を売却しました",
                TotalSellPrice = totalSellPrice,
                RemainingMoney = player.Money
            };
        }

        /// <summary>
        /// ユニークアイテムを強化する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="uniqueItemId">強化するユニークアイテムのID</param>
        /// <param name="enhancementCost">強化にかかる費用</param>
        /// <returns>強化結果</returns>
        public async Task<ItemEnhancementResult> EnhanceUniqueItemAsync(int playerUserId, int uniqueItemId, int enhancementCost = 1000)
        {
            // プレイヤーの存在確認
            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
            {
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "プレイヤーが存在しません"
                };
            }

            // ユニークアイテムの確認
            var uniqueItem = await _uniqueItemRepository.GetUniqueItemAsync(uniqueItemId);
            if (uniqueItem == null || uniqueItem.PlayerUserId != playerUserId)
            {
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "アイテムが存在しないか、所有者が異なります"
                };
            }

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(uniqueItem.ItemMasterId);
            if (itemInfo == null)
            {
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "アイテム情報が見つかりません"
                };
            }

            // 強化レベル上限チェック
            const int maxEnhancementLevel = 10;
            if (uniqueItem.EnhancementLevel >= maxEnhancementLevel)
            {
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "これ以上強化できません"
                };
            }

            // 強化費用チェック
            var totalCost = enhancementCost * (uniqueItem.EnhancementLevel + 1);
            if (player.Money < totalCost)
            {
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "強化費用が不足しています"
                };
            }

            // 強化実行
            uniqueItem.EnhancementLevel++;
            
            // レアリティに応じてステータスボーナスを付与
            var bonusMultiplier = itemInfo.Rarity;
            uniqueItem.AttackBonus += bonusMultiplier * 2;
            uniqueItem.DefenseBonus += bonusMultiplier * 2;
            uniqueItem.HealthBonus += bonusMultiplier * 5;
            uniqueItem.ManaBonus += bonusMultiplier * 3;

            await _uniqueItemRepository.UpdateUniqueItemAsync(uniqueItem);

            // プレイヤーの所持金を減らす
            player.Money -= totalCost;
            await _playerRepository.UpdatePlayerAsync(player);

            return new ItemEnhancementResult
            {
                Success = true,
                Message = $"{itemInfo.Name}を+{uniqueItem.EnhancementLevel}に強化しました",
                NewEnhancementLevel = uniqueItem.EnhancementLevel,
                EnhancementCost = totalCost,
                RemainingMoney = player.Money
            };
        }
    }
}