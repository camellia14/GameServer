using GameServer.Entities;

namespace GameServer.Repositories.Interfaces
{
    /// <summary>
    /// バフリポジトリのインターフェース
    /// バフエンティティのデータアクセス操作を定義する
    /// </summary>
    public interface IBuffRepository
    {
        /// <summary>
        /// 指定されたキャラクターのアクティブなバフ一覧を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>アクティブなバフのリスト</returns>
        Task<List<BuffEntity>> GetActiveBuffsByCharacterAsync(int characterId);

        /// <summary>
        /// 全てのアクティブなバフを取得する
        /// </summary>
        /// <returns>全アクティブバフのリスト</returns>
        Task<List<BuffEntity>> GetAllActiveBuffsAsync();

        /// <summary>
        /// 指定されたIDのバフを取得する
        /// </summary>
        /// <param name="buffId">バフID</param>
        /// <returns>バフエンティティ、存在しない場合はnull</returns>
        Task<BuffEntity?> GetBuffAsync(int buffId);

        /// <summary>
        /// 指定されたキャラクターの特定バフマスターIDのバフを取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffMasterId">バフマスターID</param>
        /// <returns>バフエンティティ、存在しない場合はnull</returns>
        Task<BuffEntity?> GetBuffByMasterIdAsync(int characterId, int buffMasterId);

        /// <summary>
        /// バフを作成する
        /// </summary>
        /// <param name="buff">作成するバフエンティティ</param>
        /// <returns>作成されたバフエンティティ</returns>
        Task<BuffEntity> CreateBuffAsync(BuffEntity buff);

        /// <summary>
        /// バフを更新する
        /// </summary>
        /// <param name="buff">更新するバフエンティティ</param>
        /// <returns>更新されたバフエンティティ</returns>
        Task<BuffEntity> UpdateBuffAsync(BuffEntity buff);

        /// <summary>
        /// バフを削除する
        /// </summary>
        /// <param name="buffId">削除するバフのID</param>
        /// <returns>削除完了を示すタスク</returns>
        Task DeleteBuffAsync(int buffId);

        /// <summary>
        /// 指定されたキャラクターの指定タイプのバフ一覧を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffType">バフタイプ</param>
        /// <returns>指定タイプのバフリスト</returns>
        Task<List<BuffEntity>> GetBuffsByTypeAsync(int characterId, BuffType buffType);

        /// <summary>
        /// 期限切れのバフを削除する
        /// </summary>
        /// <returns>削除されたバフの数</returns>
        Task<int> DeleteExpiredBuffsAsync();

        /// <summary>
        /// 指定されたキャラクターの全てのバフを削除する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>削除されたバフの数</returns>
        Task<int> DeleteAllBuffsByCharacterAsync(int characterId);
    }
}