namespace GameServer.MasterData
{
    /// <summary>
    /// マスターデータ管理クラス
    /// 全てのマスターデータへのアクセスを提供する
    /// </summary>
    public class MasterDataManager
    {
        private static MasterDataManager? _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// アイテムマスターデータ
        /// </summary>
        public ItemMaster ItemMaster { get; private set; }

        /// <summary>
        /// MasterDataManagerのプライベートコンストラクタ
        /// </summary>
        private MasterDataManager()
        {
            ItemMaster = new ItemMaster();
        }

        /// <summary>
        /// MasterDataManagerのシングルトンインスタンスを取得する
        /// </summary>
        public static MasterDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new MasterDataManager();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// マスターデータが初期化されているかを確認する
        /// </summary>
        /// <returns>初期化済みの場合はtrue、未初期化の場合はfalse</returns>
        public bool IsInitialized()
        {
            return ItemMaster.Count > 0;
        }

        /// <summary>
        /// 全てのマスターデータをリセットする（主にテスト用）
        /// </summary>
        public void Reset()
        {
            ItemMaster = new ItemMaster();
        }

        /// <summary>
        /// マスターデータの統計情報を取得する
        /// </summary>
        /// <returns>統計情報の辞書</returns>
        public Dictionary<string, int> GetStatistics()
        {
            return new Dictionary<string, int>
            {
                { "TotalItems", ItemMaster.Count },
                { "StackItems", ItemMaster.GetStackItems().Count },
                { "UniqueItems", ItemMaster.GetUniqueItems().Count },
                { "PurchasableItems", ItemMaster.GetPurchasableItems().Count },
                { "SellableItems", ItemMaster.GetSellableItems().Count }
            };
        }
    }
}