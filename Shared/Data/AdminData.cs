using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// クライアント・サーバー間で送受信される管理者情報のデータクラス
    /// ユーザーの権限と停止状態を表現する
    /// </summary>
    [MessagePackObject]
    public class AdminData
    {
        /// <summary>
        /// 対象ユーザーのID
        /// </summary>
        [Key(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ユーザーの権限レベル（0: Player, 1: Admin）
        /// </summary>
        [Key(1)]
        public int Role { get; set; }

        /// <summary>
        /// アカウントの状態（0: Active, 1: Suspended, 2: Deleted）
        /// </summary>
        [Key(2)]
        public int Status { get; set; }

        /// <summary>
        /// 停止処理を実行した管理者のユーザーID
        /// </summary>
        [Key(3)]
        public int? SuspendedByUserId { get; set; }

        /// <summary>
        /// 停止された日時
        /// </summary>
        [Key(4)]
        public DateTime? SuspendedAt { get; set; }

        /// <summary>
        /// 停止理由
        /// </summary>
        [Key(5)]
        public string? SuspensionReason { get; set; }
    }

    /// <summary>
    /// ユーザー停止要求を表すデータクラス
    /// 停止処理のパラメータを格納する
    /// </summary>
    [MessagePackObject]
    public class SuspensionRequest
    {
        /// <summary>
        /// 停止対象のユーザーID
        /// </summary>
        [Key(0)]
        public int TargetUserId { get; set; }

        /// <summary>
        /// 停止処理を実行する管理者のユーザーID
        /// </summary>
        [Key(1)]
        public int AdminUserId { get; set; }

        /// <summary>
        /// 停止理由
        /// </summary>
        [Key(2)]
        public string Reason { get; set; } = string.Empty;
    }
}