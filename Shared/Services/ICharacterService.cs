using MagicOnion;
using Shared.Data;

namespace Shared.Services
{
    /// <summary>
    /// キャラクター関連のRPCサービスインターフェース
    /// クライアント・サーバー間のキャラクター操作API定義
    /// </summary>
    public interface ICharacterService : IService<ICharacterService>
    {
        /// <summary>
        /// 指定されたIDのキャラクター情報を取得する
        /// </summary>
        /// <param name="characterId">取得したいキャラクターのID</param>
        /// <returns>キャラクター情報、存在しない場合はnull</returns>
        UnaryResult<CharacterData?> GetCharacter(int characterId);

        /// <summary>
        /// 指定されたプレイヤーが所有するすべてのキャラクターを取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>キャラクターリスト</returns>
        UnaryResult<List<CharacterData>> GetPlayerCharacters(int playerUserId);

        /// <summary>
        /// 新しいキャラクターを作成する
        /// </summary>
        /// <param name="playerUserId">キャラクターを作成するプレイヤーのユーザーID</param>
        /// <param name="characterName">作成するキャラクターの名前</param>
        /// <returns>作成されたキャラクター情報、失敗した場合はnull</returns>
        UnaryResult<CharacterData?> CreateCharacter(int playerUserId, string characterName);

        /// <summary>
        /// キャラクターを指定した位置に移動させる
        /// </summary>
        /// <param name="characterId">移動させるキャラクターのID</param>
        /// <param name="positionX">移動先のX座標</param>
        /// <param name="positionY">移動先のY座標</param>
        /// <returns>移動後のキャラクター情報、失敗した場合はnull</returns>
        UnaryResult<CharacterData?> MoveCharacter(int characterId, float positionX, float positionY);

        /// <summary>
        /// 指定されたキャラクターを削除する
        /// </summary>
        /// <param name="characterId">削除するキャラクターのID</param>
        /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
        UnaryResult<bool> DeleteCharacter(int characterId);

        /// <summary>
        /// 現在のプレイヤーが所有するすべてのキャラクターを取得する
        /// プレイヤーIDはリクエストコンテキストから自動取得される
        /// </summary>
        /// <returns>現在のプレイヤーのキャラクターリスト</returns>
        UnaryResult<List<CharacterData>> GetMyCharacters();

        /// <summary>
        /// キャラクターの詳細移動（方向と速度を指定）
        /// </summary>
        /// <param name="request">移動リクエスト</param>
        /// <returns>移動結果</returns>
        UnaryResult<OperationResult<CharacterData>> MoveCharacterAdvanced(CharacterMoveRequest request);

        /// <summary>
        /// キャラクターの回転（向きを変更）
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="rotation">新しい回転角度（度数）</param>
        /// <returns>更新されたキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> RotateCharacter(int characterId, float rotation);

        /// <summary>
        /// キャラクターが他のキャラクターを攻撃する
        /// </summary>
        /// <param name="request">攻撃リクエスト</param>
        /// <returns>攻撃結果</returns>
        UnaryResult<OperationResult<AttackResult>> AttackCharacter(CharacterAttackRequest request);

        /// <summary>
        /// スキルを使用する
        /// </summary>
        /// <param name="request">スキル使用リクエスト</param>
        /// <returns>スキル使用結果</returns>
        UnaryResult<OperationResult<SkillResult>> UseSkill(SkillUseRequest request);

        /// <summary>
        /// キャラクターのHPを回復する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="healAmount">回復量</param>
        /// <returns>回復後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> HealCharacter(int characterId, int healAmount);

        /// <summary>
        /// キャラクターのMPを回復する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="mpAmount">回復量</param>
        /// <returns>回復後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> RestoreMp(int characterId, int mpAmount);

        /// <summary>
        /// キャラクターを復活させる
        /// </summary>
        /// <param name="request">復活リクエスト</param>
        /// <returns>復活後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> ReviveCharacter(CharacterReviveRequest request);

        /// <summary>
        /// キャラクターのレベルアップ
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="experienceGained">獲得経験値</param>
        /// <returns>レベルアップ後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> GainExperience(int characterId, int experienceGained);

        /// <summary>
        /// キャラクターのステータスを更新する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="hp">新しいHP（nullの場合は変更しない）</param>
        /// <param name="mp">新しいMP（nullの場合は変更しない）</param>
        /// <param name="attackPower">新しい攻撃力（nullの場合は変更しない）</param>
        /// <param name="defensePower">新しい防御力（nullの場合は変更しない）</param>
        /// <returns>更新後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> UpdateCharacterStats(int characterId, int? hp = null, int? mp = null, int? attackPower = null, int? defensePower = null);

        /// <summary>
        /// キャラクターの統計情報を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>統計情報</returns>
        UnaryResult<CharacterStatistics> GetCharacterStatistics(int characterId);

        /// <summary>
        /// 指定された範囲内のキャラクターを検索する
        /// </summary>
        /// <param name="centerX">中心X座標</param>
        /// <param name="centerY">中心Y座標</param>
        /// <param name="radius">検索半径</param>
        /// <param name="includeDeadCharacters">死亡したキャラクターも含めるかどうか</param>
        /// <returns>範囲内のキャラクター一覧</returns>
        UnaryResult<List<CharacterData>> GetCharactersInRange(float centerX, float centerY, float radius, bool includeDeadCharacters = false);

        /// <summary>
        /// オンライン中のキャラクター一覧を取得する
        /// </summary>
        /// <param name="maxResults">最大取得数</param>
        /// <returns>オンラインキャラクター一覧</returns>
        UnaryResult<List<CharacterData>> GetOnlineCharacters(int maxResults = 50);

        /// <summary>
        /// キャラクターのバフ一覧を取得する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>適用中のバフ一覧</returns>
        UnaryResult<List<BuffData>> GetCharacterBuffs(int characterId);

        /// <summary>
        /// キャラクターにバフを適用する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffMasterId">バフマスターID</param>
        /// <param name="level">バフレベル</param>
        /// <param name="durationSeconds">持続時間（秒、-1で永続）</param>
        /// <returns>適用結果</returns>
        UnaryResult<OperationResult<BuffData>> ApplyBuff(int characterId, int buffMasterId, int level = 1, int durationSeconds = -1);

        /// <summary>
        /// キャラクターからバフを除去する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="buffId">除去するバフID</param>
        /// <returns>除去結果</returns>
        UnaryResult<OperationResult> RemoveBuff(int characterId, int buffId);

        /// <summary>
        /// キャラクターの全バフを除去する
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>除去結果</returns>
        UnaryResult<OperationResult> RemoveAllBuffs(int characterId);

        /// <summary>
        /// キャラクターを指定した位置にテレポートする
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="targetX">目標X座標</param>
        /// <param name="targetY">目標Y座標</param>
        /// <returns>テレポート後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> TeleportCharacter(int characterId, float targetX, float targetY);

        /// <summary>
        /// キャラクターを安全な位置に緊急脱出させる
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <returns>脱出後のキャラクター情報</returns>
        UnaryResult<OperationResult<CharacterData>> EmergencyEscape(int characterId);
    }
}