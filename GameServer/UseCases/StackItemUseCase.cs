using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories.Interfaces;
using GameServer.Services;
using Shared.Data;

namespace GameServer.UseCases
{
    /// <summary>
    /// スタックアイテムに関するビジネスロジックを提供するユースケースクラス
    /// </summary>
    public class StackItemUseCase
    {
        private readonly IStackItemRepository _stackItemRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ScriptExecutor _scriptExecutor;

        /// <summary>
        /// StackItemUseCaseのコンストラクタ
        /// </summary>
        /// <param name="stackItemRepository">スタックアイテムリポジトリ</param>
        /// <param name="playerRepository">プレイヤーリポジトリ</param>
        /// <param name="scriptExecutor">スクリプト実行サービス</param>
        public StackItemUseCase(IStackItemRepository stackItemRepository, IPlayerRepository playerRepository, ScriptExecutor scriptExecutor)
        {
            _stackItemRepository = stackItemRepository;
            _playerRepository = playerRepository;
            _scriptExecutor = scriptExecutor;
        }

        /// <summary>
        /// プレイヤーが所有するスタックアイテム一覧を取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>スタックアイテムデータのリスト</returns>
        public async Task<List<StackItemData>> GetPlayerStackItemsAsync(int playerUserId)
        {
            var stackItems = await _stackItemRepository.GetPlayerStackItemsAsync(playerUserId);
            var itemMaster = MasterDataManager.Instance.ItemMaster;

            return stackItems.Select(item =>
            {
                var itemInfo = itemMaster.GetById(item.ItemMasterId);
                return new StackItemData
                {
                    StackItemId = item.StackItemId,
                    ItemMasterId = item.ItemMasterId,
                    ItemName = itemInfo?.Name ?? "Unknown Item",
                    Count = item.Count,
                    MaxStackCount = itemInfo?.MaxStackCount ?? 1
                };
            }).ToList();
        }

        /// <summary>
        /// スタックアイテムを使用する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">使用するアイテムのマスターID</param>
        /// <param name="useCount">使用する個数</param>
        /// <returns>使用結果</returns>
        public async Task<ItemUseResult> UseStackItemAsync(int playerUserId, int itemMasterId, int useCount = 1)
        {
            if (useCount <= 0)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "使用個数は1以上である必要があります"
                };
            }

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

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(itemMasterId);
            if (itemInfo == null)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテムが存在しません"
                };
            }

            if (itemInfo.ItemType != ItemType.Stack)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "このアイテムはスタックアイテムではありません"
                };
            }

            // プレイヤーの所有確認
            var stackItem = await _stackItemRepository.GetStackItemAsync(playerUserId, itemMasterId);
            if (stackItem == null || stackItem.Count < useCount)
            {
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテムが不足しています"
                };
            }

            // スクリプト実行
            ScriptExecutionResult? scriptResult = null;
            if (!string.IsNullOrEmpty(itemInfo.EffectScript))
            {
                scriptResult = await _scriptExecutor.ExecuteAsync(itemInfo.EffectScript, playerUserId, itemMasterId);
                if (!scriptResult.Success)
                {
                    return new ItemUseResult
                    {
                        Success = false,
                        Message = $"アイテム効果の実行に失敗しました: {scriptResult.Message}"
                    };
                }
            }

            // アイテム個数を減らす
            stackItem.Count -= useCount;
            if (stackItem.Count <= 0)
            {
                await _stackItemRepository.DeleteStackItemAsync(stackItem.StackItemId);
            }
            else
            {
                await _stackItemRepository.UpdateStackItemAsync(stackItem);
            }

            return new ItemUseResult
            {
                Success = true,
                Message = scriptResult?.Message ?? $"{itemInfo.Name}を{useCount}個使用しました",
                RemainingCount = Math.Max(0, stackItem.Count)
            };
        }

        /// <summary>
        /// スタックアイテムを購入する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">購入するアイテムのマスターID</param>
        /// <param name="purchaseCount">購入する個数</param>
        /// <returns>購入結果</returns>
        public async Task<ItemPurchaseResult> PurchaseStackItemAsync(int playerUserId, int itemMasterId, int purchaseCount = 1)
        {
            if (purchaseCount <= 0)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "購入個数は1以上である必要があります"
                };
            }

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

            if (itemInfo.ItemType != ItemType.Stack)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "このアイテムはスタックアイテムではありません"
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

            // 購入金額の計算
            var totalPrice = itemInfo.Price * purchaseCount;
            if (player.Money < totalPrice)
            {
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "お金が不足しています"
                };
            }

            // 既存のスタックアイテムを確認
            var existingStackItem = await _stackItemRepository.GetStackItemAsync(playerUserId, itemMasterId);
            if (existingStackItem != null)
            {
                // スタック上限確認
                if (existingStackItem.Count + purchaseCount > itemInfo.MaxStackCount)
                {
                    return new ItemPurchaseResult
                    {
                        Success = false,
                        Message = $"スタック上限を超えています（上限: {itemInfo.MaxStackCount}）"
                    };
                }

                existingStackItem.Count += purchaseCount;
                await _stackItemRepository.UpdateStackItemAsync(existingStackItem);
            }
            else
            {
                // 新規作成
                var newStackItem = new StackItemEntity
                {
                    PlayerUserId = playerUserId,
                    ItemMasterId = itemMasterId,
                    Count = purchaseCount
                };
                await _stackItemRepository.CreateStackItemAsync(newStackItem);
            }

            // プレイヤーの所持金を減らす
            player.Money -= totalPrice;
            await _playerRepository.UpdatePlayerAsync(player);

            return new ItemPurchaseResult
            {
                Success = true,
                Message = $"{itemInfo.Name}を{purchaseCount}個購入しました",
                TotalPrice = totalPrice,
                RemainingMoney = player.Money
            };
        }

        /// <summary>
        /// スタックアイテムを売却する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">売却するアイテムのマスターID</param>
        /// <param name="sellCount">売却する個数</param>
        /// <returns>売却結果</returns>
        public async Task<ItemSellResult> SellStackItemAsync(int playerUserId, int itemMasterId, int sellCount = 1)
        {
            if (sellCount <= 0)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "売却個数は1以上である必要があります"
                };
            }

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

            // アイテムマスターデータの確認
            var itemMaster = MasterDataManager.Instance.ItemMaster;
            var itemInfo = itemMaster.GetById(itemMasterId);
            if (itemInfo == null)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテムが存在しません"
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

            // プレイヤーの所有確認
            var stackItem = await _stackItemRepository.GetStackItemAsync(playerUserId, itemMasterId);
            if (stackItem == null || stackItem.Count < sellCount)
            {
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテムが不足しています"
                };
            }

            // 売却金額の計算
            var totalSellPrice = itemInfo.SellPrice * sellCount;

            // アイテム個数を減らす
            stackItem.Count -= sellCount;
            if (stackItem.Count <= 0)
            {
                await _stackItemRepository.DeleteStackItemAsync(stackItem.StackItemId);
            }
            else
            {
                await _stackItemRepository.UpdateStackItemAsync(stackItem);
            }

            // プレイヤーの所持金を増やす
            player.Money += totalSellPrice;
            await _playerRepository.UpdatePlayerAsync(player);

            return new ItemSellResult
            {
                Success = true,
                Message = $"{itemInfo.Name}を{sellCount}個売却しました",
                TotalSellPrice = totalSellPrice,
                RemainingMoney = player.Money,
                RemainingCount = Math.Max(0, stackItem.Count)
            };
        }
    }
}