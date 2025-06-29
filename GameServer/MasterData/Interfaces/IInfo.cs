namespace GameServer.MasterData.Interfaces
{
    /// <summary>
    /// マスターデータの１レコード分のインターフェース
    /// すべてのマスターデータレコードが持つべき基本的な識別子を定義する
    /// </summary>
    public interface IInfo
    {
        /// <summary>
        /// マスターデータレコードの一意識別子
        /// </summary>
        int Id { get; }
    }
}