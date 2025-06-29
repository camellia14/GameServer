using GameServer.MasterData.Interfaces;
using GameServer.Entities;

namespace GameServer.MasterData
{
    /// <summary>
    /// バフマスターデータクラス
    /// 全バフの基本情報を管理する
    /// </summary>
    public class BuffMaster : BaseMaster<BuffInfo>
    {
        /// <summary>
        /// 指定されたバフタイプのバフ一覧を取得する
        /// </summary>
        /// <param name="buffType">取得したいバフの種類</param>
        /// <returns>指定された種類のバフリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetByType(BuffType buffType)
        {
            return GetAll().Where(buff => buff.BuffType == buffType).ToList().AsReadOnly();
        }

        /// <summary>
        /// 有利効果（バフ）一覧を取得する
        /// </summary>
        /// <returns>有利効果のリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetBuffs()
        {
            return GetByType(BuffType.Buff);
        }

        /// <summary>
        /// 不利効果（デバフ）一覧を取得する
        /// </summary>
        /// <returns>不利効果のリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetDebuffs()
        {
            return GetByType(BuffType.Debuff);
        }

        /// <summary>
        /// 状態異常一覧を取得する
        /// </summary>
        /// <returns>状態異常のリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetAbnormalEffects()
        {
            return GetByType(BuffType.Abnormal);
        }

        /// <summary>
        /// スタック可能なバフ一覧を取得する
        /// </summary>
        /// <returns>スタック可能なバフのリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetStackableBuffs()
        {
            return GetAll().Where(buff => buff.CanStack && buff.MaxStackCount > 1).ToList().AsReadOnly();
        }

        /// <summary>
        /// 除去可能なバフ一覧を取得する
        /// </summary>
        /// <returns>除去可能なバフのリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetDispelableBuffs()
        {
            return GetAll().Where(buff => buff.CanDispel).ToList().AsReadOnly();
        }

        /// <summary>
        /// 永続効果のバフ一覧を取得する
        /// </summary>
        /// <returns>永続効果のバフリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetPermanentBuffs()
        {
            return GetAll().Where(buff => buff.IsPermanent()).ToList().AsReadOnly();
        }

        /// <summary>
        /// スタック減少するバフ一覧を取得する
        /// </summary>
        /// <returns>スタック減少するバフのリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetStackDecreasingBuffs()
        {
            return GetAll().Where(buff => buff.HasStackDecrease()).ToList().AsReadOnly();
        }

        /// <summary>
        /// 優先度順でソートされたバフ一覧を取得する
        /// </summary>
        /// <returns>優先度順のバフリスト</returns>
        public IReadOnlyCollection<BuffInfo> GetBuffsByPriority()
        {
            return GetAll().OrderByDescending(buff => buff.Priority).ThenBy(buff => buff.Name).ToList().AsReadOnly();
        }
    }
}