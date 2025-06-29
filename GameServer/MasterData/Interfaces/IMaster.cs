namespace GameServer.MasterData.Interfaces
{
    /// <summary>
    /// マスターデータのインターフェース
    /// ゲーム開始時に全てメモリ上にロードされるデータを管理する
    /// </summary>
    /// <typeparam name="TInfo">マスターデータの個別レコード型</typeparam>
    public interface IMaster<TInfo> where TInfo : IInfo
    {
        /// <summary>
        /// 指定されたIDのマスターデータレコードを取得する
        /// </summary>
        /// <param name="id">取得するデータのID</param>
        /// <returns>マスターデータレコード、存在しない場合はnull</returns>
        TInfo? GetById(int id);

        /// <summary>
        /// すべてのマスターデータレコードを取得する
        /// </summary>
        /// <returns>全マスターデータレコードのコレクション</returns>
        IReadOnlyCollection<TInfo> GetAll();

        /// <summary>
        /// マスターデータが指定されたIDで存在するかを確認する
        /// </summary>
        /// <param name="id">確認するID</param>
        /// <returns>存在する場合はtrue、しない場合はfalse</returns>
        bool Exists(int id);

        /// <summary>
        /// マスターデータをメモリに読み込む
        /// </summary>
        /// <param name="dataSource">データソース（通常はデータベースから取得したデータ）</param>
        void LoadData(IEnumerable<TInfo> dataSource);

        /// <summary>
        /// 読み込まれているマスターデータの件数を取得する
        /// </summary>
        int Count { get; }
    }
}