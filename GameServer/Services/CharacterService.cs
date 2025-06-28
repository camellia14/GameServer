using MagicOnion;
using MagicOnion.Server;
using Shared.Services;
using Shared.Data;
using GameServer.UseCases;

namespace GameServer.Services
{
    /// <summary>
    /// キャラクター関連のRPCサービスを提供するクラス
    /// キャラクターのCRUD操作と移動処理を担当する
    /// </summary>
    public class CharacterService : ServiceBase<ICharacterService>, ICharacterService
    {
        private readonly CharacterUseCase _characterUseCase;

        /// <summary>
        /// CharacterServiceのコンストラクタ
        /// </summary>
        /// <param name="characterUseCase">キャラクタービジネスロジック</param>
        public CharacterService(CharacterUseCase characterUseCase)
        {
            _characterUseCase = characterUseCase;
        }

        /// <summary>
        /// 指定されたIDのキャラクター情報を取得する
        /// </summary>
        /// <param name="characterId">取得したいキャラクターのID</param>
        /// <returns>キャラクター情報、存在しない場合はnull</returns>
        public async UnaryResult<CharacterData?> GetCharacter(int characterId)
        {
            try
            {
                return await _characterUseCase.GetCharacterAsync(characterId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCharacter: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 指定されたプレイヤーが所有するすべてのキャラクターを取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>キャラクターリスト（空リストの場合もある）</returns>
        public async UnaryResult<List<CharacterData>> GetPlayerCharacters(int playerUserId)
        {
            try
            {
                return await _characterUseCase.GetPlayerCharactersAsync(playerUserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPlayerCharacters: {ex.Message}");
                return new List<CharacterData>();
            }
        }

        /// <summary>
        /// 新しいキャラクターを作成する
        /// </summary>
        /// <param name="playerUserId">キャラクターを作成するプレイヤーのユーザーID</param>
        /// <param name="characterName">作成するキャラクターの名前</param>
        /// <returns>作成されたキャラクター情報、失敗した場合はnull</returns>
        public async UnaryResult<CharacterData?> CreateCharacter(int playerUserId, string characterName)
        {
            try
            {
                return await _characterUseCase.CreateCharacterAsync(playerUserId, characterName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCharacter: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// キャラクターを指定した位置に移動させる
        /// </summary>
        /// <param name="characterId">移動させるキャラクターのID</param>
        /// <param name="positionX">移動先のX座標</param>
        /// <param name="positionY">移動先のY座標</param>
        /// <returns>移動後のキャラクター情報、失敗した場合はnull</returns>
        public async UnaryResult<CharacterData?> MoveCharacter(int characterId, float positionX, float positionY)
        {
            try
            {
                return await _characterUseCase.MoveCharacterAsync(characterId, positionX, positionY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MoveCharacter: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 指定されたキャラクターを削除する
        /// </summary>
        /// <param name="characterId">削除するキャラクターのID</param>
        /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
        public async UnaryResult<bool> DeleteCharacter(int characterId)
        {
            try
            {
                await _characterUseCase.DeleteCharacterAsync(characterId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCharacter: {ex.Message}");
                return false;
            }
        }
    }
}