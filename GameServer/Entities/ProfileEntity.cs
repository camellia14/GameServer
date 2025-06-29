using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// プロフィールエンティティクラス
    /// ユーザーのプロフィール情報を管理する
    /// </summary>
    [Table("profiles")]
    public class ProfileEntity
    {
        /// <summary>
        /// プロフィールの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }

        /// <summary>
        /// 関連するアカウントID
        /// </summary>
        [Required]
        public int AccountId { get; set; }

        /// <summary>
        /// プロフィール画像のURL
        /// </summary>
        [MaxLength(500)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 自己紹介文
        /// </summary>
        [MaxLength(1000)]
        public string Biography { get; set; } = string.Empty;

        /// <summary>
        /// 生年月日
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 居住国
        /// </summary>
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 好きなゲームジャンル
        /// </summary>
        [MaxLength(200)]
        public string FavoriteGenre { get; set; } = string.Empty;

        /// <summary>
        /// プレイ時間の合計（分単位）
        /// </summary>
        public int TotalPlayTimeMinutes { get; set; } = 0;

        /// <summary>
        /// プレイヤーレベル
        /// </summary>
        public int PlayerLevel { get; set; } = 1;

        /// <summary>
        /// 経験値
        /// </summary>
        public long Experience { get; set; } = 0;

        /// <summary>
        /// 獲得実績の数
        /// </summary>
        public int AchievementCount { get; set; } = 0;

        /// <summary>
        /// プロフィールの公開設定
        /// </summary>
        [Required]
        public ProfilePrivacy Privacy { get; set; } = ProfilePrivacy.Public;

        /// <summary>
        /// オンライン状態の表示設定
        /// </summary>
        [Required]
        public bool ShowOnlineStatus { get; set; } = true;

        /// <summary>
        /// フレンド申請の受付設定
        /// </summary>
        [Required]
        public bool AcceptFriendRequests { get; set; } = true;

        /// <summary>
        /// レコード作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// レコード更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// アカウントエンティティとの関連
        /// </summary>
        [ForeignKey(nameof(AccountId))]
        public virtual AccountEntity? Account { get; set; }

        /// <summary>
        /// 年齢を計算する
        /// </summary>
        /// <returns>年齢、生年月日が設定されていない場合はnull</returns>
        public int? GetAge()
        {
            if (!DateOfBirth.HasValue) return null;
            
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Value.Year;
            if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
            
            return age >= 0 ? age : null;
        }

        /// <summary>
        /// プレイ時間を時間単位で取得する
        /// </summary>
        /// <returns>プレイ時間（時間）</returns>
        public double GetPlayTimeHours()
        {
            return TotalPlayTimeMinutes / 60.0;
        }
    }

    /// <summary>
    /// プロフィールの公開設定を定義する列挙型
    /// </summary>
    public enum ProfilePrivacy
    {
        /// <summary>
        /// 公開（誰でも閲覧可能）
        /// </summary>
        Public = 0,

        /// <summary>
        /// フレンドのみ
        /// </summary>
        FriendsOnly = 1,

        /// <summary>
        /// 非公開
        /// </summary>
        Private = 2
    }
}