using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    /// <summary>
    /// ゲーム内のキャラクターを表すエンティティクラス
    /// プレイヤーが複数のキャラクターを所有できる
    /// </summary>
    public class CharacterEntity
    {
        /// <summary>
        /// キャラクターの一意識別子
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }

        /// <summary>
        /// このキャラクターを所有するプレイヤーのユーザーID
        /// </summary>
        [Required]
        public int PlayerUserId { get; set; }

        /// <summary>
        /// キャラクターの名前（最大50文字）
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// キャラクターのレベル（初期値は1）
        /// </summary>
        [Required]
        public int Level { get; set; } = 1;

        /// <summary>
        /// キャラクターのX座標位置
        /// </summary>
        [Required]
        public float PositionX { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターのY座標位置
        /// </summary>
        [Required]
        public float PositionY { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターの作成日時
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// キャラクターの最終更新日時
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// キャラクターの現在HP
        /// </summary>
        [Required]
        public int CurrentHp { get; set; } = 100;

        /// <summary>
        /// キャラクターの最大HP
        /// </summary>
        [Required]
        public int MaxHp { get; set; } = 100;

        /// <summary>
        /// キャラクターの現在MP
        /// </summary>
        [Required]
        public int CurrentMp { get; set; } = 50;

        /// <summary>
        /// キャラクターの最大MP
        /// </summary>
        [Required]
        public int MaxMp { get; set; } = 50;

        /// <summary>
        /// キャラクターの攻撃力
        /// </summary>
        [Required]
        public int AttackPower { get; set; } = 10;

        /// <summary>
        /// キャラクターの防御力
        /// </summary>
        [Required]
        public int DefensePower { get; set; } = 5;

        /// <summary>
        /// キャラクターの移動速度
        /// </summary>
        [Required]
        public float MovementSpeed { get; set; } = 5.0f;

        /// <summary>
        /// キャラクターの向いている方向（度数）
        /// </summary>
        [Required]
        public float Rotation { get; set; } = 0.0f;

        /// <summary>
        /// キャラクターが生きているかどうか
        /// </summary>
        [Required]
        public bool IsAlive { get; set; } = true;

        /// <summary>
        /// 現在の経験値
        /// </summary>
        [Required]
        public int Experience { get; set; } = 0;

        /// <summary>
        /// このキャラクターを所有するプレイヤーエンティティへの参照
        /// </summary>
        [ForeignKey("PlayerUserId")]
        public virtual PlayerEntity? Player { get; set; }

        /// <summary>
        /// キャラクターの位置を更新する
        /// </summary>
        /// <param name="x">新しいX座標</param>
        /// <param name="y">新しいY座標</param>
        /// <param name="rotation">新しい回転角度（任意）</param>
        public void UpdatePosition(float x, float y, float? rotation = null)
        {
            PositionX = x;
            PositionY = y;
            if (rotation.HasValue)
                Rotation = rotation.Value;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">受けるダメージ量</param>
        /// <returns>実際に受けたダメージ量</returns>
        public int TakeDamage(int damage)
        {
            if (!IsAlive || damage <= 0)
                return 0;

            int actualDamage = Math.Min(damage, CurrentHp);
            CurrentHp -= actualDamage;
            
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                IsAlive = false;
            }
            
            UpdatedAt = DateTime.UtcNow;
            return actualDamage;
        }

        /// <summary>
        /// HPを回復する
        /// </summary>
        /// <param name="healAmount">回復量</param>
        /// <returns>実際に回復したHP量</returns>
        public int Heal(int healAmount)
        {
            if (healAmount <= 0)
                return 0;

            int actualHeal = Math.Min(healAmount, MaxHp - CurrentHp);
            CurrentHp += actualHeal;
            UpdatedAt = DateTime.UtcNow;
            return actualHeal;
        }

        /// <summary>
        /// MPを消費する
        /// </summary>
        /// <param name="mpCost">消費MP</param>
        /// <returns>消費に成功した場合はtrue</returns>
        public bool ConsumeMp(int mpCost)
        {
            if (mpCost <= 0 || CurrentMp < mpCost)
                return false;

            CurrentMp -= mpCost;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// MPを回復する
        /// </summary>
        /// <param name="mpAmount">回復量</param>
        /// <returns>実際に回復したMP量</returns>
        public int RestoreMp(int mpAmount)
        {
            if (mpAmount <= 0)
                return 0;

            int actualRestore = Math.Min(mpAmount, MaxMp - CurrentMp);
            CurrentMp += actualRestore;
            UpdatedAt = DateTime.UtcNow;
            return actualRestore;
        }

        /// <summary>
        /// 経験値を獲得してレベルアップを判定する
        /// </summary>
        /// <param name="exp">獲得経験値</param>
        /// <returns>レベルアップした場合は新しいレベル、しなかった場合は0</returns>
        public int GainExperience(int exp)
        {
            if (exp <= 0)
                return 0;

            Experience += exp;
            int newLevel = CalculateLevelFromExperience(Experience);
            
            if (newLevel > Level)
            {
                int oldLevel = Level;
                Level = newLevel;
                LevelUp(newLevel - oldLevel);
                UpdatedAt = DateTime.UtcNow;
                return newLevel;
            }
            
            UpdatedAt = DateTime.UtcNow;
            return 0;
        }

        /// <summary>
        /// キャラクターを復活させる
        /// </summary>
        /// <param name="hpRatio">復活時のHP割合（0.0-1.0）</param>
        /// <param name="x">復活位置X座標（任意）</param>
        /// <param name="y">復活位置Y座標（任意）</param>
        public void Revive(float hpRatio = 0.5f, float? x = null, float? y = null)
        {
            if (IsAlive)
                return;

            IsAlive = true;
            CurrentHp = (int)(MaxHp * Math.Clamp(hpRatio, 0.0f, 1.0f));
            
            if (x.HasValue && y.HasValue)
            {
                PositionX = x.Value;
                PositionY = y.Value;
            }
            
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// ステータスを更新する
        /// </summary>
        /// <param name="hp">新しいHP（任意）</param>
        /// <param name="mp">新しいMP（任意）</param>
        /// <param name="attackPower">新しい攻撃力（任意）</param>
        /// <param name="defensePower">新しい防御力（任意）</param>
        public void UpdateStats(int? hp = null, int? mp = null, int? attackPower = null, int? defensePower = null)
        {
            if (hp.HasValue)
            {
                MaxHp = Math.Max(1, hp.Value);
                CurrentHp = Math.Min(CurrentHp, MaxHp);
            }
            
            if (mp.HasValue)
            {
                MaxMp = Math.Max(0, mp.Value);
                CurrentMp = Math.Min(CurrentMp, MaxMp);
            }
            
            if (attackPower.HasValue)
                AttackPower = Math.Max(1, attackPower.Value);
                
            if (defensePower.HasValue)
                DefensePower = Math.Max(0, defensePower.Value);
                
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 経験値からレベルを計算する
        /// </summary>
        /// <param name="exp">経験値</param>
        /// <returns>レベル</returns>
        private int CalculateLevelFromExperience(int exp)
        {
            // 簡単なレベル計算式: レベル = sqrt(経験値 / 100) + 1
            return (int)Math.Floor(Math.Sqrt(exp / 100.0)) + 1;
        }

        /// <summary>
        /// レベルアップ時のステータス上昇処理
        /// </summary>
        /// <param name="levelsGained">上昇したレベル数</param>
        private void LevelUp(int levelsGained)
        {
            // レベルアップごとにステータス上昇
            int hpIncrease = levelsGained * 10;
            int mpIncrease = levelsGained * 5;
            int attackIncrease = levelsGained * 2;
            int defenseIncrease = levelsGained * 1;

            MaxHp += hpIncrease;
            MaxMp += mpIncrease;
            AttackPower += attackIncrease;
            DefensePower += defenseIncrease;

            // レベルアップ時はHPとMPを全回復
            CurrentHp = MaxHp;
            CurrentMp = MaxMp;
        }

        /// <summary>
        /// 攻撃範囲内にいるかを判定する
        /// </summary>
        /// <param name="targetX">対象のX座標</param>
        /// <param name="targetY">対象のY座標</param>
        /// <param name="range">攻撃範囲</param>
        /// <returns>範囲内にいる場合はtrue</returns>
        public bool IsInAttackRange(float targetX, float targetY, float range)
        {
            float distance = (float)Math.Sqrt(Math.Pow(targetX - PositionX, 2) + Math.Pow(targetY - PositionY, 2));
            return distance <= range;
        }

        /// <summary>
        /// 他のキャラクターとの距離を計算する
        /// </summary>
        /// <param name="other">対象キャラクター</param>
        /// <returns>距離</returns>
        public float GetDistanceTo(CharacterEntity other)
        {
            return (float)Math.Sqrt(Math.Pow(other.PositionX - PositionX, 2) + Math.Pow(other.PositionY - PositionY, 2));
        }
    }
}