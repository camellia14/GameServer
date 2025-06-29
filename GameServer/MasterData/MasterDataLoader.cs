using GameServer.DB;
using GameServer.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.MasterData
{
    /// <summary>
    /// マスターデータローダークラス
    /// ゲーム開始時にデータベースからマスターデータをメモリに読み込む
    /// </summary>
    public class MasterDataLoader
    {
        private readonly AppDbContext _context;
        private readonly ItemMaster _itemMaster;

        /// <summary>
        /// マスターデータローダーのコンストラクタ
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="itemMaster">アイテムマスターデータ</param>
        public MasterDataLoader(AppDbContext context, ItemMaster itemMaster)
        {
            _context = context;
            _itemMaster = itemMaster;
        }

        /// <summary>
        /// 全てのマスターデータをデータベースから読み込む
        /// </summary>
        /// <returns>読み込み完了を示すタスク</returns>
        public async Task LoadAllMasterDataAsync()
        {
            Console.WriteLine("Loading master data...");

            await LoadItemMasterAsync();

            Console.WriteLine("Master data loading completed.");
        }

        /// <summary>
        /// アイテムマスターデータを読み込む
        /// </summary>
        /// <returns>読み込み完了を示すタスク</returns>
        private async Task LoadItemMasterAsync()
        {
            try
            {
                // データベースからアイテムマスターデータを読み込む
                // 実際の実装では、マスターデータ用のテーブルから読み込む
                // ここでは仮のデータを作成
                var itemInfoList = new List<ItemInfo>
                {
                    new ItemInfo
                    {
                        Id = 1,
                        Name = "体力回復ポーション",
                        Description = "HPを50回復する",
                        ItemType = ItemType.Stack,
                        Price = 100,
                        SellPrice = 50,
                        MaxStackCount = 99,
                        EffectScript = "heal_hp_50",
                        Rarity = 1
                    },
                    new ItemInfo
                    {
                        Id = 2,
                        Name = "マナ回復ポーション",
                        Description = "MPを30回復する",
                        ItemType = ItemType.Stack,
                        Price = 80,
                        SellPrice = 40,
                        MaxStackCount = 99,
                        EffectScript = "heal_mp_30",
                        Rarity = 1
                    },
                    new ItemInfo
                    {
                        Id = 3,
                        Name = "鉄の剣",
                        Description = "基本的な鉄製の剣",
                        ItemType = ItemType.Unique,
                        Price = 500,
                        SellPrice = 250,
                        MaxStackCount = 1,
                        EffectScript = null,
                        Rarity = 2
                    },
                    new ItemInfo
                    {
                        Id = 4,
                        Name = "レザーアーマー",
                        Description = "軽量な革製の鎧",
                        ItemType = ItemType.Unique,
                        Price = 400,
                        SellPrice = 200,
                        MaxStackCount = 1,
                        EffectScript = null,
                        Rarity = 2
                    },
                    new ItemInfo
                    {
                        Id = 5,
                        Name = "ドラゴンソード",
                        Description = "伝説のドラゴンから作られた剣",
                        ItemType = ItemType.Unique,
                        Price = 10000,
                        SellPrice = 5000,
                        MaxStackCount = 1,
                        EffectScript = null,
                        Rarity = 5
                    }
                };

                _itemMaster.LoadData(itemInfoList);
                Console.WriteLine($"Loaded {_itemMaster.Count} item master records.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading item master data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// マスターデータの読み込み状況を確認する
        /// </summary>
        /// <returns>読み込み状況の文字列</returns>
        public string GetLoadStatus()
        {
            return $"ItemMaster: {_itemMaster.Count} records loaded";
        }
    }
}