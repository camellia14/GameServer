using MagicOnion;
using MagicOnion.Server;
using GameServer.Repositories.Interfaces;
using GameServer.UseCases;
using Shared.Services;
using Shared.Data;
using System.Linq;

namespace GameServer.Services
{
    /// <summary>
    /// アイテム関連のRPCサービスを提供するクラス
    /// スタックアイテムとユニークアイテムの操作を担当する
    /// </summary>
    public class ItemService : ServiceBase<IItemService>, IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly StackItemUseCase _stackItemUseCase;
        private readonly UniqueItemUseCase _uniqueItemUseCase;

        /// <summary>
        /// ItemServiceのコンストラクタ
        /// </summary>
        /// <param name="itemRepository">アイテムリポジトリ</param>
        /// <param name="stackItemUseCase">スタックアイテムユースケース</param>
        /// <param name="uniqueItemUseCase">ユニークアイテムユースケース</param>
        public ItemService(IItemRepository itemRepository, StackItemUseCase stackItemUseCase, UniqueItemUseCase uniqueItemUseCase)
        {
            _itemRepository = itemRepository;
            _stackItemUseCase = stackItemUseCase;
            _uniqueItemUseCase = uniqueItemUseCase;
        }

        /// <summary>
        /// 既存のアイテム取得メソッド
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns>アイテムデータ</returns>
        public async UnaryResult<ItemData?> GetItem(int id)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(id);
                if (item == null) return null;

                return new ItemData
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Stock = item.Stock
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetItem: {ex.Message}");
                return null;
            }
        }

        // スタックアイテム関連メソッド
        /// <summary>
        /// プレイヤーのスタックアイテム一覧を取得する
        /// </summary>
        /// <returns>スタックアイテムのリスト</returns>
        public async UnaryResult<List<StackItemData>> GetMyStackItems()
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _stackItemUseCase.GetPlayerStackItemsAsync(playerUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyStackItems: {ex.Message}");
                return new List<StackItemData>();
            }
        }

        /// <summary>
        /// スタックアイテムを使用する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="useCount">使用個数</param>
        /// <returns>使用結果</returns>
        public async UnaryResult<ItemUseResult> UseStackItem(int itemMasterId, int useCount)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _stackItemUseCase.UseStackItemAsync(playerUserId, itemMasterId, useCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UseStackItem: {ex.Message}");
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテムの使用中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// スタックアイテムを購入する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="purchaseCount">購入個数</param>
        /// <returns>購入結果</returns>
        public async UnaryResult<ItemPurchaseResult> PurchaseStackItem(int itemMasterId, int purchaseCount)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _stackItemUseCase.PurchaseStackItemAsync(playerUserId, itemMasterId, purchaseCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PurchaseStackItem: {ex.Message}");
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "アイテムの購入中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// スタックアイテムを売却する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <param name="sellCount">売却個数</param>
        /// <returns>売却結果</returns>
        public async UnaryResult<ItemSellResult> SellStackItem(int itemMasterId, int sellCount)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _stackItemUseCase.SellStackItemAsync(playerUserId, itemMasterId, sellCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SellStackItem: {ex.Message}");
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテムの売却中にエラーが発生しました"
                };
            }
        }

        // ユニークアイテム関連メソッド
        /// <summary>
        /// プレイヤーのユニークアイテム一覧を取得する
        /// </summary>
        /// <returns>ユニークアイテムのリスト</returns>
        public async UnaryResult<List<UniqueItemData>> GetMyUniqueItems()
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _uniqueItemUseCase.GetPlayerUniqueItemsAsync(playerUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyUniqueItems: {ex.Message}");
                return new List<UniqueItemData>();
            }
        }

        /// <summary>
        /// ユニークアイテムを使用する（装備/装備解除）
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>使用結果</returns>
        public async UnaryResult<ItemUseResult> UseUniqueItem(int uniqueItemId)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _uniqueItemUseCase.UseUniqueItemAsync(playerUserId, uniqueItemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UseUniqueItem: {ex.Message}");
                return new ItemUseResult
                {
                    Success = false,
                    Message = "アイテムの使用中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// ユニークアイテムを購入する
        /// </summary>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>購入結果</returns>
        public async UnaryResult<ItemPurchaseResult> PurchaseUniqueItem(int itemMasterId)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _uniqueItemUseCase.PurchaseUniqueItemAsync(playerUserId, itemMasterId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PurchaseUniqueItem: {ex.Message}");
                return new ItemPurchaseResult
                {
                    Success = false,
                    Message = "アイテムの購入中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// ユニークアイテムを売却する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>売却結果</returns>
        public async UnaryResult<ItemSellResult> SellUniqueItem(int uniqueItemId)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _uniqueItemUseCase.SellUniqueItemAsync(playerUserId, uniqueItemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SellUniqueItem: {ex.Message}");
                return new ItemSellResult
                {
                    Success = false,
                    Message = "アイテムの売却中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// ユニークアイテムを強化する
        /// </summary>
        /// <param name="uniqueItemId">ユニークアイテムID</param>
        /// <returns>強化結果</returns>
        public async UnaryResult<ItemEnhancementResult> EnhanceUniqueItem(int uniqueItemId)
        {
            try
            {
                var playerUserId = GetCurrentPlayerUserId();
                return await _uniqueItemUseCase.EnhanceUniqueItemAsync(playerUserId, uniqueItemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EnhanceUniqueItem: {ex.Message}");
                return new ItemEnhancementResult
                {
                    Success = false,
                    Message = "アイテムの強化中にエラーが発生しました"
                };
            }
        }

        /// <summary>
        /// リクエストコンテキストから現在のプレイヤーIDを取得する
        /// </summary>
        /// <returns>プレイヤーのユーザーID</returns>
        /// <exception cref="UnauthorizedAccessException">プレイヤーIDが取得できない場合</exception>
        private int GetCurrentPlayerUserId()
        {
            // MagicOnionのコンテキストからプレイヤーIDを取得
            // 実際の認証実装では、JWTトークンやセッション情報から取得することが多い
            // 今回はヘッダーから "Player-Id" を取得する簡易実装
            if (Context.CallContext.RequestHeaders.Any(h => h.Key.Equals("player-id", StringComparison.OrdinalIgnoreCase)))
            {
                var playerIdHeader = Context.CallContext.RequestHeaders.First(h => h.Key.Equals("player-id", StringComparison.OrdinalIgnoreCase));
                if (int.TryParse(playerIdHeader.Value, out int playerId))
                {
                    return playerId;
                }
            }
            
            // ヘッダーにプレイヤーIDが無い場合はデフォルト値として1を返す（テスト用）
            // 本番環境では適切な認証処理を実装する必要がある
            return 1;
        }
    }
}
