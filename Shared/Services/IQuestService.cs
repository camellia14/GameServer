using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// クエスト関連のRPCサービスインターフェース
    /// クエストの進捗管理、完了処理、報酬付与を提供する
    /// </summary>
    public interface IQuestService : IService<IQuestService>
    {
        /// <summary>
        /// プレイヤーのクエスト一覧を取得する
        /// </summary>
        /// <returns>クエスト一覧</returns>
        UnaryResult<List<QuestData>> GetMyQuests();

        /// <summary>
        /// 利用可能なクエスト一覧を取得する
        /// </summary>
        /// <returns>開始可能なクエストリスト</returns>
        UnaryResult<List<QuestData>> GetAvailableQuests();

        /// <summary>
        /// クエストを開始する
        /// </summary>
        /// <param name="request">クエスト開始リクエスト</param>
        /// <returns>開始結果</returns>
        UnaryResult<OperationResult> StartQuest(QuestStartRequest request);

        /// <summary>
        /// クエストの進捗を更新する
        /// </summary>
        /// <param name="request">進捗更新リクエスト</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateQuestProgress(QuestProgressRequest request);

        /// <summary>
        /// クエストを完了する
        /// </summary>
        /// <param name="questId">完了するクエストID</param>
        /// <returns>完了結果</returns>
        UnaryResult<OperationResult> CompleteQuest(int questId);

        /// <summary>
        /// クエスト報酬を受け取る
        /// </summary>
        /// <param name="request">報酬受取リクエスト</param>
        /// <returns>報酬結果</returns>
        UnaryResult<QuestRewardResult> ClaimQuestReward(QuestRewardClaimRequest request);

        /// <summary>
        /// クエストを放棄する
        /// </summary>
        /// <param name="questId">放棄するクエストID</param>
        /// <returns>放棄結果</returns>
        UnaryResult<OperationResult> AbandonQuest(int questId);

        /// <summary>
        /// 完了済みクエスト一覧を取得する
        /// </summary>
        /// <returns>完了済みクエストリスト</returns>
        UnaryResult<List<QuestData>> GetCompletedQuests();

        /// <summary>
        /// デイリークエストをリセットする（管理者機能）
        /// </summary>
        /// <returns>リセット結果</returns>
        UnaryResult<OperationResult> ResetDailyQuests();

        /// <summary>
        /// 期限切れクエストを処理する（システム機能）
        /// </summary>
        /// <returns>処理結果</returns>
        UnaryResult<OperationResult> ProcessExpiredQuests();

        /// <summary>
        /// クエスト統計情報を取得する
        /// </summary>
        /// <returns>統計情報</returns>
        UnaryResult<QuestStatistics> GetQuestStatistics();
    }

    /// <summary>
    /// クエスト統計情報のデータクラス
    /// </summary>
    [MessagePack.MessagePackObject]
    public class QuestStatistics
    {
        /// <summary>
        /// 進行中のクエスト数
        /// </summary>
        [MessagePack.Key(0)]
        public int InProgressCount { get; set; }

        /// <summary>
        /// 完了済みクエスト数
        /// </summary>
        [MessagePack.Key(1)]
        public int CompletedCount { get; set; }

        /// <summary>
        /// 報酬受取済みクエスト数
        /// </summary>
        [MessagePack.Key(2)]
        public int RewardClaimedCount { get; set; }

        /// <summary>
        /// 放棄したクエスト数
        /// </summary>
        [MessagePack.Key(3)]
        public int AbandonedCount { get; set; }

        /// <summary>
        /// 期限切れクエスト数
        /// </summary>
        [MessagePack.Key(4)]
        public int ExpiredCount { get; set; }

        /// <summary>
        /// 合計獲得経験値
        /// </summary>
        [MessagePack.Key(5)]
        public long TotalExperienceGained { get; set; }

        /// <summary>
        /// 合計獲得金額
        /// </summary>
        [MessagePack.Key(6)]
        public int TotalMoneyGained { get; set; }
    }
}