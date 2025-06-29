using GameServer.MasterData.Interfaces;

namespace GameServer.MasterData
{
    /// <summary>
    /// マスターデータの基底実装クラス
    /// IMasterインターフェースの共通実装を提供する
    /// </summary>
    /// <typeparam name="TInfo">マスターデータの個別レコード型</typeparam>
    public abstract class BaseMaster<TInfo> : IMaster<TInfo> where TInfo : IInfo
    {
        private readonly Dictionary<int, TInfo> _data = new();

        /// <summary>
        /// 指定されたIDのマスターデータレコードを取得する
        /// </summary>
        /// <param name="id">取得するデータのID</param>
        /// <returns>マスターデータレコード、存在しない場合はnull</returns>
        public TInfo? GetById(int id)
        {
            return _data.TryGetValue(id, out var info) ? info : default;
        }

        /// <summary>
        /// すべてのマスターデータレコードを取得する
        /// </summary>
        /// <returns>全マスターデータレコードのコレクション</returns>
        public IReadOnlyCollection<TInfo> GetAll()
        {
            return _data.Values.ToList().AsReadOnly();
        }

        /// <summary>
        /// マスターデータが指定されたIDで存在するかを確認する
        /// </summary>
        /// <param name="id">確認するID</param>
        /// <returns>存在する場合はtrue、しない場合はfalse</returns>
        public bool Exists(int id)
        {
            return _data.ContainsKey(id);
        }

        /// <summary>
        /// マスターデータをメモリに読み込む
        /// </summary>
        /// <param name="dataSource">データソース（通常はデータベースから取得したデータ）</param>
        public virtual void LoadData(IEnumerable<TInfo> dataSource)
        {
            _data.Clear();
            foreach (var info in dataSource)
            {
                _data[info.Id] = info;
            }
        }

        /// <summary>
        /// 読み込まれているマスターデータの件数を取得する
        /// </summary>
        public int Count => _data.Count;
    }
}