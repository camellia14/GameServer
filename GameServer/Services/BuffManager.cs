using GameServer.Entities;
using GameServer.MasterData;
using GameServer.Repositories.Interfaces;

namespace GameServer.Services
{
    /// <summary>
    /// バフデータを管理するクラス
    /// キャラクターに付与されるバフの状態を管理し、時間経過処理を行う
    /// </summary>
    public class BuffManager
    {
        private readonly IBuffRepository _buffRepository;
        private readonly ILogger<BuffManager>? _logger;

        /// <summary>
        /// BuffManagerのコンストラクタ
        /// </summary>
        /// <param name="buffRepository">バフリポジトリ</param>
        /// <param name="logger">ロガー（オプション）</param>
        public BuffManager(IBuffRepository buffRepository, ILogger<BuffManager>? logger = null)
        {
            _buffRepository = buffRepository;
            _logger = logger;
        }

        /// <summary>
        /// キャラクターにバフを付与する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffMasterId">バフマスターID</param>
        /// <param name="level">バフレベル</param>
        /// <param name="durationSeconds">継続時間（秒）、永続効果の場合は-1</param>
        /// <param name="stackCount">スタック数</param>
        /// <returns>付与されたバフエンティティ</returns>
        public async Task<BuffEntity?> ApplyBuffAsync(int characterId, int buffMasterId, int level = 1, int? durationSeconds = null, int stackCount = 1)
        {
            try
            {
                var buffMaster = MasterDataManager.Instance.BuffMaster;
                var buffInfo = buffMaster.GetById(buffMasterId);
                if (buffInfo == null)
                {
                    _logger?.LogWarning($"BuffInfo not found for BuffMasterId: {buffMasterId}");
                    return null;
                }

                // 既存のバフをチェック
                var existingBuffs = await _buffRepository.GetActiveBuffsByCharacterAsync(characterId);
                var existingBuff = existingBuffs.FirstOrDefault(b => b.BuffMasterId == buffMasterId);

                if (existingBuff != null && buffInfo.CanStack)
                {
                    // スタック処理
                    var newStackCount = Math.Min(existingBuff.StackCount + stackCount, buffInfo.MaxStackCount);
                    existingBuff.StackCount = newStackCount;
                    existingBuff.BuffLevel = Math.Max(existingBuff.BuffLevel, level);
                    
                    // 継続時間の更新（より長い方を採用）
                    if (durationSeconds.HasValue && durationSeconds.Value > 0)
                    {
                        var newEndTime = DateTime.UtcNow.AddSeconds(durationSeconds.Value);
                        if (!existingBuff.EndTime.HasValue || newEndTime > existingBuff.EndTime.Value)
                        {
                            existingBuff.EndTime = newEndTime;
                            existingBuff.DurationSeconds = durationSeconds.Value;
                        }
                    }

                    return await _buffRepository.UpdateBuffAsync(existingBuff);
                }
                else if (existingBuff != null)
                {
                    // スタック不可の場合は上書き
                    existingBuff.BuffLevel = level;
                    existingBuff.StackCount = stackCount;
                    existingBuff.StartTime = DateTime.UtcNow;
                    
                    var finalDuration = durationSeconds ?? buffInfo.DefaultDurationSeconds;
                    if (finalDuration > 0)
                    {
                        existingBuff.EndTime = DateTime.UtcNow.AddSeconds(finalDuration);
                        existingBuff.DurationSeconds = finalDuration;
                    }
                    else
                    {
                        existingBuff.EndTime = null;
                        existingBuff.DurationSeconds = -1;
                    }

                    return await _buffRepository.UpdateBuffAsync(existingBuff);
                }
                else
                {
                    // 新規バフ作成
                    var finalDuration = durationSeconds ?? buffInfo.DefaultDurationSeconds;
                    var buff = new BuffEntity
                    {
                        CharacterId = characterId,
                        BuffMasterId = buffMasterId,
                        BuffLevel = level,
                        BuffType = buffInfo.BuffType,
                        StackCount = Math.Min(stackCount, buffInfo.MaxStackCount),
                        StartTime = DateTime.UtcNow,
                        DurationSeconds = finalDuration,
                        StackDecreaseIntervalSeconds = buffInfo.StackDecreaseIntervalSeconds
                    };

                    if (finalDuration > 0)
                    {
                        buff.EndTime = DateTime.UtcNow.AddSeconds(finalDuration);
                    }

                    if (buffInfo.StackDecreaseIntervalSeconds > 0)
                    {
                        buff.NextStackDecreaseTime = DateTime.UtcNow.AddSeconds(buffInfo.StackDecreaseIntervalSeconds);
                    }

                    return await _buffRepository.CreateBuffAsync(buff);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to apply buff {buffMasterId} to character {characterId}");
                return null;
            }
        }

        /// <summary>
        /// キャラクターからバフを除去する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffMasterId">バフマスターID</param>
        /// <param name="stackCount">除去するスタック数（0の場合は全て除去）</param>
        /// <returns>除去に成功した場合はtrue</returns>
        public async Task<bool> RemoveBuffAsync(int characterId, int buffMasterId, int stackCount = 0)
        {
            try
            {
                var existingBuffs = await _buffRepository.GetActiveBuffsByCharacterAsync(characterId);
                var buff = existingBuffs.FirstOrDefault(b => b.BuffMasterId == buffMasterId);

                if (buff == null) return false;

                var buffMaster = MasterDataManager.Instance.BuffMaster;
                var buffInfo = buffMaster.GetById(buffMasterId);
                if (buffInfo != null && !buffInfo.CanDispel)
                {
                    _logger?.LogWarning($"Attempted to remove non-dispellable buff {buffMasterId}");
                    return false;
                }

                if (stackCount > 0 && buff.StackCount > stackCount)
                {
                    // 部分的なスタック除去
                    buff.StackCount -= stackCount;
                    await _buffRepository.UpdateBuffAsync(buff);
                }
                else
                {
                    // 完全除去
                    await _buffRepository.DeleteBuffAsync(buff.BuffId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to remove buff {buffMasterId} from character {characterId}");
                return false;
            }
        }

        /// <summary>
        /// キャラクターの全てのバフを取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>アクティブなバフのリスト</returns>
        public async Task<List<BuffEntity>> GetCharacterBuffsAsync(int characterId)
        {
            return await _buffRepository.GetActiveBuffsByCharacterAsync(characterId);
        }

        /// <summary>
        /// 期限切れのバフとスタック減少処理を実行する
        /// </summary>
        /// <param name="characterId">キャラクターID（nullの場合は全キャラクター）</param>
        /// <returns>処理されたバフの数</returns>
        public async Task<int> ProcessBuffUpdatesAsync(int? characterId = null)
        {
            try
            {
                var buffs = characterId.HasValue 
                    ? await _buffRepository.GetActiveBuffsByCharacterAsync(characterId.Value)
                    : await _buffRepository.GetAllActiveBuffsAsync();

                int processedCount = 0;
                var currentTime = DateTime.UtcNow;

                foreach (var buff in buffs)
                {
                    bool updated = false;

                    // 期限切れチェック
                    if (buff.IsExpired())
                    {
                        await _buffRepository.DeleteBuffAsync(buff.BuffId);
                        processedCount++;
                        continue;
                    }

                    // スタック減少チェック
                    if (buff.ShouldDecreaseStack())
                    {
                        buff.StackCount--;
                        if (buff.StackCount <= 0)
                        {
                            await _buffRepository.DeleteBuffAsync(buff.BuffId);
                        }
                        else
                        {
                            buff.NextStackDecreaseTime = currentTime.AddSeconds(buff.StackDecreaseIntervalSeconds);
                            await _buffRepository.UpdateBuffAsync(buff);
                        }
                        processedCount++;
                        updated = true;
                    }

                    if (!updated && buff.StackCount <= 0)
                    {
                        await _buffRepository.DeleteBuffAsync(buff.BuffId);
                        processedCount++;
                    }
                }

                return processedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to process buff updates");
                return 0;
            }
        }

        /// <summary>
        /// キャラクターの特定タイプのバフを全て除去する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffType">除去するバフタイプ</param>
        /// <returns>除去されたバフの数</returns>
        public async Task<int> RemoveBuffsByTypeAsync(int characterId, BuffType buffType)
        {
            try
            {
                var buffs = await _buffRepository.GetActiveBuffsByCharacterAsync(characterId);
                var targetBuffs = buffs.Where(b => b.BuffType == buffType).ToList();

                int removedCount = 0;
                foreach (var buff in targetBuffs)
                {
                    var buffMaster = MasterDataManager.Instance.BuffMaster;
                    var buffInfo = buffMaster.GetById(buff.BuffMasterId);
                    if (buffInfo != null && !buffInfo.CanDispel) continue;

                    await _buffRepository.DeleteBuffAsync(buff.BuffId);
                    removedCount++;
                }

                return removedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to remove buffs of type {buffType} from character {characterId}");
                return 0;
            }
        }

        /// <summary>
        /// キャラクターのステータス修正値を計算する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>ステータス修正値</returns>
        public async Task<BuffStatusModifiers> CalculateStatusModifiersAsync(int characterId)
        {
            var modifiers = new BuffStatusModifiers();
            
            try
            {
                var buffs = await _buffRepository.GetActiveBuffsByCharacterAsync(characterId);
                var buffMaster = MasterDataManager.Instance.BuffMaster;

                foreach (var buff in buffs)
                {
                    var buffInfo = buffMaster.GetById(buff.BuffMasterId);
                    if (buffInfo == null) continue;

                    var multiplier = buff.StackCount * buff.BuffLevel;
                    modifiers.AttackModifier += buffInfo.AttackModifier * multiplier;
                    modifiers.DefenseModifier += buffInfo.DefenseModifier * multiplier;
                    modifiers.HealthModifier += buffInfo.HealthModifier * multiplier;
                    modifiers.ManaModifier += buffInfo.ManaModifier * multiplier;
                    modifiers.SpeedModifier += buffInfo.SpeedModifier * multiplier;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to calculate status modifiers for character {characterId}");
            }

            return modifiers;
        }
    }

    /// <summary>
    /// バフによるステータス修正値を格納するクラス
    /// </summary>
    public class BuffStatusModifiers
    {
        /// <summary>
        /// 攻撃力修正値（%）
        /// </summary>
        public float AttackModifier { get; set; } = 0f;

        /// <summary>
        /// 防御力修正値（%）
        /// </summary>
        public float DefenseModifier { get; set; } = 0f;

        /// <summary>
        /// HP修正値（%）
        /// </summary>
        public float HealthModifier { get; set; } = 0f;

        /// <summary>
        /// MP修正値（%）
        /// </summary>
        public float ManaModifier { get; set; } = 0f;

        /// <summary>
        /// 移動速度修正値（%）
        /// </summary>
        public float SpeedModifier { get; set; } = 0f;
    }
}