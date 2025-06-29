using GameServer.MasterData.Interfaces;

namespace GameServer.MasterData
{
    /// <summary>
    /// バフのマスターデータ１レコード
    /// バフの基本情報と設定を定義する
    /// </summary>
    public class BuffInfo : IInfo
    {
        /// <summary>
        /// バフマスターID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// バフ名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// バフの説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// バフのタイプ
        /// </summary>
        public BuffType BuffType { get; set; }

        /// <summary>
        /// 最大スタック数
        /// </summary>
        public int MaxStackCount { get; set; } = 1;

        /// <summary>
        /// デフォルトの継続時間（秒）、永続効果の場合は-1
        /// </summary>
        public int DefaultDurationSeconds { get; set; } = -1;

        /// <summary>
        /// スタック減少間隔（秒）、スタック減少しない場合は0
        /// </summary>
        public int StackDecreaseIntervalSeconds { get; set; } = 0;

        /// <summary>
        /// バフアイコンのパス
        /// </summary>
        public string IconPath { get; set; } = string.Empty;

        /// <summary>
        /// バフの優先度（表示順序用）
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// バフが重複可能かどうか
        /// </summary>
        public bool CanStack { get; set; } = true;

        /// <summary>
        /// バフが除去可能かどうか
        /// </summary>
        public bool CanDispel { get; set; } = true;

        /// <summary>
        /// 攻撃力への影響（%）
        /// </summary>
        public float AttackModifier { get; set; } = 0f;

        /// <summary>
        /// 防御力への影響（%）
        /// </summary>
        public float DefenseModifier { get; set; } = 0f;

        /// <summary>
        /// HPへの影響（%）
        /// </summary>
        public float HealthModifier { get; set; } = 0f;

        /// <summary>
        /// MPへの影響（%）
        /// </summary>
        public float ManaModifier { get; set; } = 0f;

        /// <summary>
        /// 移動速度への影響（%）
        /// </summary>
        public float SpeedModifier { get; set; } = 0f;

        /// <summary>
        /// バフが永続効果かどうかを判定する
        /// </summary>
        /// <returns>永続効果の場合はtrue</returns>
        public bool IsPermanent()
        {
            return DefaultDurationSeconds == -1;
        }

        /// <summary>
        /// スタック減少するバフかどうかを判定する
        /// </summary>
        /// <returns>スタック減少する場合はtrue</returns>
        public bool HasStackDecrease()
        {
            return StackDecreaseIntervalSeconds > 0;
        }
    }
}