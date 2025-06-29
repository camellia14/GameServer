using MessagePack;

namespace Shared.Data
{
    /// <summary>
    /// クライアント・サーバー間で送受信されるキャラクター情報のデータクラス
    /// MessagePackでシリアライズされる
    /// </summary>
    [MessagePackObject]
    public class CharacterData
    {
        /// <summary>
        /// キャラクターの一意識別子
        /// </summary>
        [Key(0)]
        public int CharacterId { get; set; }

        /// <summary>
        /// このキャラクターを所有するプレイヤーのユーザーID
        /// </summary>
        [Key(1)]
        public int PlayerUserId { get; set; }

        /// <summary>
        /// キャラクター名
        /// </summary>
        [Key(2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// キャラクターのレベル
        /// </summary>
        [Key(3)]
        public int Level { get; set; } = 1;

        /// <summary>
        /// キャラクターのX座標位置
        /// </summary>
        [Key(4)]
        public float PositionX { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターのY座標位置
        /// </summary>
        [Key(5)]
        public float PositionY { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターの作成日時
        /// </summary>
        [Key(6)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// キャラクターの最終更新日時
        /// </summary>
        [Key(7)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// キャラクターの現在HP
        /// </summary>
        [Key(8)]
        public int CurrentHp { get; set; } = 100;

        /// <summary>
        /// キャラクターの最大HP
        /// </summary>
        [Key(9)]
        public int MaxHp { get; set; } = 100;

        /// <summary>
        /// キャラクターの現在MP
        /// </summary>
        [Key(10)]
        public int CurrentMp { get; set; } = 50;

        /// <summary>
        /// キャラクターの最大MP
        /// </summary>
        [Key(11)]
        public int MaxMp { get; set; } = 50;

        /// <summary>
        /// キャラクターの攻撃力
        /// </summary>
        [Key(12)]
        public int AttackPower { get; set; } = 10;

        /// <summary>
        /// キャラクターの防御力
        /// </summary>
        [Key(13)]
        public int DefensePower { get; set; } = 5;

        /// <summary>
        /// キャラクターの移動速度
        /// </summary>
        [Key(14)]
        public float MovementSpeed { get; set; } = 5.0f;

        /// <summary>
        /// キャラクターの向いている方向（度数）
        /// </summary>
        [Key(15)]
        public float Rotation { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターが生きているかどうか
        /// </summary>
        [Key(16)]
        public bool IsAlive { get; set; } = true;

        /// <summary>
        /// 現在の経験値
        /// </summary>
        [Key(17)]
        public int Experience { get; set; } = 0;
    }

    /// <summary>
    /// キャラクター移動リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class CharacterMoveRequest
    {
        /// <summary>
        /// 移動するキャラクターID
        /// </summary>
        [Key(0)]
        public int CharacterId { get; set; }

        /// <summary>
        /// 目標X座標
        /// </summary>
        [Key(1)]
        public float TargetX { get; set; }

        /// <summary>
        /// 目標Y座標
        /// </summary>
        [Key(2)]
        public float TargetY { get; set; }

        /// <summary>
        /// 移動速度の倍率
        /// </summary>
        [Key(3)]
        public float SpeedMultiplier { get; set; } = 1.0f;
    }

    /// <summary>
    /// キャラクター攻撃リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class CharacterAttackRequest
    {
        /// <summary>
        /// 攻撃するキャラクターID
        /// </summary>
        [Key(0)]
        public int AttackerCharacterId { get; set; }

        /// <summary>
        /// 攻撃対象のキャラクターID
        /// </summary>
        [Key(1)]
        public int TargetCharacterId { get; set; }

        /// <summary>
        /// 使用するスキルID（0の場合は通常攻撃）
        /// </summary>
        [Key(2)]
        public int SkillId { get; set; } = 0;

        /// <summary>
        /// 攻撃位置X座標（範囲攻撃の場合）
        /// </summary>
        [Key(3)]
        public float? AttackX { get; set; }

        /// <summary>
        /// 攻撃位置Y座標（範囲攻撃の場合）
        /// </summary>
        [Key(4)]
        public float? AttackY { get; set; }
    }

    /// <summary>
    /// 攻撃結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class AttackResult
    {
        /// <summary>
        /// 攻撃者のキャラクターID
        /// </summary>
        [Key(0)]
        public int AttackerCharacterId { get; set; }

        /// <summary>
        /// 攻撃対象のキャラクターID
        /// </summary>
        [Key(1)]
        public int TargetCharacterId { get; set; }

        /// <summary>
        /// 与えたダメージ
        /// </summary>
        [Key(2)]
        public int Damage { get; set; }

        /// <summary>
        /// クリティカルヒットかどうか
        /// </summary>
        [Key(3)]
        public bool IsCritical { get; set; }

        /// <summary>
        /// 攻撃が命中したかどうか
        /// </summary>
        [Key(4)]
        public bool IsHit { get; set; }

        /// <summary>
        /// 対象が倒されたかどうか
        /// </summary>
        [Key(5)]
        public bool IsTargetDefeated { get; set; }

        /// <summary>
        /// 使用されたスキルID
        /// </summary>
        [Key(6)]
        public int SkillId { get; set; }

        /// <summary>
        /// 攻撃後の対象のHP
        /// </summary>
        [Key(7)]
        public int TargetCurrentHp { get; set; }
    }

    /// <summary>
    /// スキル使用リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class SkillUseRequest
    {
        /// <summary>
        /// スキルを使用するキャラクターID
        /// </summary>
        [Key(0)]
        public int CharacterId { get; set; }

        /// <summary>
        /// 使用するスキルID
        /// </summary>
        [Key(1)]
        public int SkillId { get; set; }

        /// <summary>
        /// 対象キャラクターID（対象指定スキルの場合）
        /// </summary>
        [Key(2)]
        public int? TargetCharacterId { get; set; }

        /// <summary>
        /// 対象X座標（位置指定スキルの場合）
        /// </summary>
        [Key(3)]
        public float? TargetX { get; set; }

        /// <summary>
        /// 対象Y座標（位置指定スキルの場合）
        /// </summary>
        [Key(4)]
        public float? TargetY { get; set; }
    }

    /// <summary>
    /// スキル使用結果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class SkillResult
    {
        /// <summary>
        /// スキルを使用したキャラクターID
        /// </summary>
        [Key(0)]
        public int CasterCharacterId { get; set; }

        /// <summary>
        /// 使用されたスキルID
        /// </summary>
        [Key(1)]
        public int SkillId { get; set; }

        /// <summary>
        /// スキル使用が成功したかどうか
        /// </summary>
        [Key(2)]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消費したMP
        /// </summary>
        [Key(3)]
        public int MpCost { get; set; }

        /// <summary>
        /// 影響を受けたキャラクターのリスト
        /// </summary>
        [Key(4)]
        public List<SkillEffect> Effects { get; set; } = new List<SkillEffect>();

        /// <summary>
        /// エラーメッセージ（失敗した場合）
        /// </summary>
        [Key(5)]
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// スキル効果のデータクラス
    /// </summary>
    [MessagePackObject]
    public class SkillEffect
    {
        /// <summary>
        /// 効果を受けたキャラクターID
        /// </summary>
        [Key(0)]
        public int TargetCharacterId { get; set; }

        /// <summary>
        /// 効果の種類
        /// </summary>
        [Key(1)]
        public string EffectType { get; set; } = string.Empty;

        /// <summary>
        /// 効果の値
        /// </summary>
        [Key(2)]
        public int Value { get; set; }

        /// <summary>
        /// ダメージかどうか
        /// </summary>
        [Key(3)]
        public bool IsDamage { get; set; }

        /// <summary>
        /// クリティカルかどうか
        /// </summary>
        [Key(4)]
        public bool IsCritical { get; set; }
    }

    /// <summary>
    /// キャラクター復活リクエストのデータクラス
    /// </summary>
    [MessagePackObject]
    public class CharacterReviveRequest
    {
        /// <summary>
        /// 復活するキャラクターID
        /// </summary>
        [Key(0)]
        public int CharacterId { get; set; }

        /// <summary>
        /// 復活位置X座標（指定しない場合はデフォルト位置）
        /// </summary>
        [Key(1)]
        public float? ReviveX { get; set; }

        /// <summary>
        /// 復活位置Y座標（指定しない場合はデフォルト位置）
        /// </summary>
        [Key(2)]
        public float? ReviveY { get; set; }

        /// <summary>
        /// 復活時HP割合（0.0-1.0）
        /// </summary>
        [Key(3)]
        public float HpRatio { get; set; } = 0.5f;
    }

    /// <summary>
    /// キャラクター統計情報のデータクラス
    /// </summary>
    [MessagePackObject]
    public class CharacterStatistics
    {
        /// <summary>
        /// 総プレイ時間（分）
        /// </summary>
        [Key(0)]
        public int TotalPlayTimeMinutes { get; set; }

        /// <summary>
        /// 与えた総ダメージ
        /// </summary>
        [Key(1)]
        public long TotalDamageDealt { get; set; }

        /// <summary>
        /// 受けた総ダメージ
        /// </summary>
        [Key(2)]
        public long TotalDamageReceived { get; set; }

        /// <summary>
        /// 倒した敵の数
        /// </summary>
        [Key(3)]
        public int EnemiesDefeated { get; set; }

        /// <summary>
        /// 死亡回数
        /// </summary>
        [Key(4)]
        public int DeathCount { get; set; }

        /// <summary>
        /// 使用したスキル回数
        /// </summary>
        [Key(5)]
        public int SkillsUsed { get; set; }

        /// <summary>
        /// 移動距離（メートル）
        /// </summary>
        [Key(6)]
        public float DistanceTraveled { get; set; }

        /// <summary>
        /// 最高レベル
        /// </summary>
        [Key(7)]
        public int HighestLevel { get; set; }
    }
}