using GameServer.DB;
using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories
{
    /// <summary>
    /// バフリポジトリの実装クラス
    /// バフエンティティのデータアクセス操作を提供する
    /// </summary>
    public class BuffRepository : IBuffRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// BuffRepositoryのコンストラクタ
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public BuffRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 指定されたキャラクターのアクティブなバフ一覧を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>アクティブなバフのリスト</returns>
        public async Task<List<BuffEntity>> GetActiveBuffsByCharacterAsync(int characterId)
        {
            return await _context.Buffs
                .Where(b => b.CharacterId == characterId && b.IsActive)
                .OrderBy(b => b.BuffType)
                .ThenByDescending(b => b.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 全てのアクティブなバフを取得する
        /// </summary>
        /// <returns>全アクティブバフのリスト</returns>
        public async Task<List<BuffEntity>> GetAllActiveBuffsAsync()
        {
            return await _context.Buffs
                .Where(b => b.IsActive)
                .OrderBy(b => b.CharacterId)
                .ThenBy(b => b.BuffType)
                .ToListAsync();
        }

        /// <summary>
        /// 指定されたIDのバフを取得する
        /// </summary>
        /// <param name="buffId">バフID</param>
        /// <returns>バフエンティティ、存在しない場合はnull</returns>
        public async Task<BuffEntity?> GetBuffAsync(int buffId)
        {
            return await _context.Buffs
                .FirstOrDefaultAsync(b => b.BuffId == buffId && b.IsActive);
        }

        /// <summary>
        /// 指定されたキャラクターの特定バフマスターIDのバフを取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffMasterId">バフマスターID</param>
        /// <returns>バフエンティティ、存在しない場合はnull</returns>
        public async Task<BuffEntity?> GetBuffByMasterIdAsync(int characterId, int buffMasterId)
        {
            return await _context.Buffs
                .FirstOrDefaultAsync(b => b.CharacterId == characterId && b.BuffMasterId == buffMasterId && b.IsActive);
        }

        /// <summary>
        /// バフを作成する
        /// </summary>
        /// <param name="buff">作成するバフエンティティ</param>
        /// <returns>作成されたバフエンティティ</returns>
        public async Task<BuffEntity> CreateBuffAsync(BuffEntity buff)
        {
            buff.CreatedAt = DateTime.UtcNow;
            buff.UpdatedAt = DateTime.UtcNow;
            buff.IsActive = true;
            
            _context.Buffs.Add(buff);
            await _context.SaveChangesAsync();
            return buff;
        }

        /// <summary>
        /// バフを更新する
        /// </summary>
        /// <param name="buff">更新するバフエンティティ</param>
        /// <returns>更新されたバフエンティティ</returns>
        public async Task<BuffEntity> UpdateBuffAsync(BuffEntity buff)
        {
            buff.UpdatedAt = DateTime.UtcNow;
            
            _context.Buffs.Update(buff);
            await _context.SaveChangesAsync();
            return buff;
        }

        /// <summary>
        /// バフを削除する
        /// </summary>
        /// <param name="buffId">削除するバフのID</param>
        /// <returns>削除完了を示すタスク</returns>
        public async Task DeleteBuffAsync(int buffId)
        {
            var buff = await _context.Buffs.FindAsync(buffId);
            if (buff != null)
            {
                // 論理削除
                buff.IsActive = false;
                buff.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 指定されたキャラクターの指定タイプのバフ一覧を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffType">バフタイプ</param>
        /// <returns>指定タイプのバフリスト</returns>
        public async Task<List<BuffEntity>> GetBuffsByTypeAsync(int characterId, BuffType buffType)
        {
            return await _context.Buffs
                .Where(b => b.CharacterId == characterId && b.BuffType == buffType && b.IsActive)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 期限切れのバフを削除する
        /// </summary>
        /// <returns>削除されたバフの数</returns>
        public async Task<int> DeleteExpiredBuffsAsync()
        {
            var currentTime = DateTime.UtcNow;
            var expiredBuffs = await _context.Buffs
                .Where(b => b.IsActive && b.EndTime.HasValue && b.EndTime.Value <= currentTime)
                .ToListAsync();

            foreach (var buff in expiredBuffs)
            {
                buff.IsActive = false;
                buff.UpdatedAt = currentTime;
            }

            if (expiredBuffs.Any())
            {
                await _context.SaveChangesAsync();
            }

            return expiredBuffs.Count;
        }

        /// <summary>
        /// 指定されたキャラクターの全てのバフを削除する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>削除されたバフの数</returns>
        public async Task<int> DeleteAllBuffsByCharacterAsync(int characterId)
        {
            var buffs = await _context.Buffs
                .Where(b => b.CharacterId == characterId && b.IsActive)
                .ToListAsync();

            var currentTime = DateTime.UtcNow;
            foreach (var buff in buffs)
            {
                buff.IsActive = false;
                buff.UpdatedAt = currentTime;
            }

            if (buffs.Any())
            {
                await _context.SaveChangesAsync();
            }

            return buffs.Count;
        }
    }
}