using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// プロフィール関連のRPCサービスインターフェース
    /// プロフィール情報の登録、変更、管理を提供する
    /// </summary>
    public interface IProfileService : IService<IProfileService>
    {
        /// <summary>
        /// 現在のプロフィール情報を取得する
        /// </summary>
        /// <returns>プロフィール情報</returns>
        UnaryResult<ProfileData?> GetMyProfile();

        /// <summary>
        /// 指定されたユーザーのプロフィール情報を取得する
        /// </summary>
        /// <param name="accountId">取得したいユーザーのアカウントID</param>
        /// <returns>プロフィール情報</returns>
        UnaryResult<ProfileData?> GetProfile(int accountId);

        /// <summary>
        /// プロフィール情報を更新する
        /// </summary>
        /// <param name="request">プロフィール更新リクエスト</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateProfile(ProfileUpdateRequest request);

        /// <summary>
        /// プロフィール画像を更新する
        /// </summary>
        /// <param name="avatarUrl">新しいプロフィール画像のURL</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateAvatar(string avatarUrl);

        /// <summary>
        /// プレイ時間を追加する
        /// </summary>
        /// <param name="playTimeMinutes">追加するプレイ時間（分）</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> AddPlayTime(int playTimeMinutes);

        /// <summary>
        /// 経験値を追加する
        /// </summary>
        /// <param name="experience">追加する経験値</param>
        /// <returns>更新結果とレベルアップ情報</returns>
        UnaryResult<ExperienceResult> AddExperience(long experience);

        /// <summary>
        /// 実績数を更新する
        /// </summary>
        /// <param name="achievementCount">新しい実績数</param>
        /// <returns>更新結果</returns>
        UnaryResult<OperationResult> UpdateAchievementCount(int achievementCount);

        /// <summary>
        /// プロフィール検索（表示名で検索）
        /// </summary>
        /// <param name="searchTerm">検索文字列</param>
        /// <param name="maxResults">最大結果数</param>
        /// <returns>検索結果のプロフィールリスト</returns>
        UnaryResult<List<ProfileData>> SearchProfiles(string searchTerm, int maxResults = 20);

        /// <summary>
        /// ランキング取得（レベル順）
        /// </summary>
        /// <param name="topCount">取得する上位の人数</param>
        /// <returns>レベルランキングのプロフィールリスト</returns>
        UnaryResult<List<ProfileData>> GetLevelRanking(int topCount = 100);

        /// <summary>
        /// ランキング取得（プレイ時間順）
        /// </summary>
        /// <param name="topCount">取得する上位の人数</param>
        /// <returns>プレイ時間ランキングのプロフィールリスト</returns>
        UnaryResult<List<ProfileData>> GetPlayTimeRanking(int topCount = 100);
    }

    /// <summary>
    /// 経験値追加結果のデータクラス
    /// </summary>
    [MessagePack.MessagePackObject]
    public class ExperienceResult
    {
        /// <summary>
        /// 処理が成功したかどうか
        /// </summary>
        [MessagePack.Key(0)]
        public bool Success { get; set; }

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        [MessagePack.Key(1)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// レベルアップしたかどうか
        /// </summary>
        [MessagePack.Key(2)]
        public bool LeveledUp { get; set; }

        /// <summary>
        /// 新しいレベル
        /// </summary>
        [MessagePack.Key(3)]
        public int NewLevel { get; set; }

        /// <summary>
        /// 獲得した経験値
        /// </summary>
        [MessagePack.Key(4)]
        public long GainedExperience { get; set; }

        /// <summary>
        /// 現在の経験値
        /// </summary>
        [MessagePack.Key(5)]
        public long CurrentExperience { get; set; }
    }
}