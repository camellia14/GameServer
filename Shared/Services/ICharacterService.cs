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
    }
}