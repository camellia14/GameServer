using GameServer.Entities;
using GameServer.Repositories.Interfaces;
using Shared.Data;

namespace GameServer.UseCases
{
    /// <summary>
    /// キャラクター関連のビジネスロジックを管理するユースケースクラス
    /// キャラクターのCRUD操作、移動処理、データ変換を担当する
    /// </summary>
    public class CharacterUseCase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IPlayerRepository _playerRepository;

        /// <summary>
        /// CharacterUseCaseのコンストラクタ
        /// </summary>
        /// <param name="characterRepository">キャラクターデータアクセス</param>
        /// <param name="playerRepository">プレイヤーデータアクセス</param>
        public CharacterUseCase(ICharacterRepository characterRepository, IPlayerRepository playerRepository)
        {
            _characterRepository = characterRepository;
            _playerRepository = playerRepository;
        }

        /// <summary>
        /// 指定されたIDのキャラクター情報を取得する
        /// </summary>
        /// <param name="characterId">取得したいキャラクターのID</param>
        /// <returns>キャラクター情報、存在しない場合はnull</returns>
        public async Task<CharacterData?> GetCharacterAsync(int characterId)
        {
            var character = await _characterRepository.GetCharacterByIdAsync(characterId);
            if (character == null)
                return null;

            return MapToCharacterData(character);
        }

        /// <summary>
        /// 指定されたプレイヤーが所有するすべてのキャラクターを取得する
        /// </summary>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <returns>キャラクターリスト</returns>
        public async Task<List<CharacterData>> GetPlayerCharactersAsync(int playerUserId)
        {
            var characters = await _characterRepository.GetCharactersByPlayerIdAsync(playerUserId);
            return characters.Select(MapToCharacterData).ToList();
        }

        /// <summary>
        /// 新しいキャラクターを作成する
        /// </summary>
        /// <param name="playerUserId">キャラクターを作成するプレイヤーのユーザーID</param>
        /// <param name="characterName">作成するキャラクターの名前</param>
        /// <returns>作成されたキャラクター情報</returns>
        /// <exception cref="ArgumentException">キャラクター名が無効な場合</exception>
        /// <exception cref="InvalidOperationException">プレイヤーが存在しない場合</exception>
        public async Task<CharacterData?> CreateCharacterAsync(int playerUserId, string characterName)
        {
            if (string.IsNullOrWhiteSpace(characterName))
                throw new ArgumentException("Character name cannot be empty");

            if (characterName.Length > 50)
                throw new ArgumentException("Character name must be 50 characters or less");

            var player = await _playerRepository.GetPlayerAsync(playerUserId);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            var character = new CharacterEntity
            {
                PlayerUserId = playerUserId,
                Name = characterName.Trim(),
                Level = 1,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            var createdCharacter = await _characterRepository.CreateCharacterAsync(character);
            return MapToCharacterData(createdCharacter);
        }

        /// <summary>
        /// キャラクターを指定した位置に移動させる
        /// </summary>
        /// <param name="characterId">移動させるキャラクターのID</param>
        /// <param name="positionX">移動先のX座標</param>
        /// <param name="positionY">移動先のY座標</param>
        /// <returns>移動後のキャラクター情報、キャラクターが存在しない場合はnull</returns>
        public async Task<CharacterData?> MoveCharacterAsync(int characterId, float positionX, float positionY)
        {
            var character = await _characterRepository.GetCharacterByIdAsync(characterId);
            if (character == null)
                return null;

            character.PositionX = positionX;
            character.PositionY = positionY;

            var updatedCharacter = await _characterRepository.UpdateCharacterAsync(character);
            return MapToCharacterData(updatedCharacter);
        }

        /// <summary>
        /// 指定されたキャラクターを削除する
        /// </summary>
        /// <param name="characterId">削除するキャラクターのID</param>
        /// <exception cref="InvalidOperationException">キャラクターが存在しない場合</exception>
        public async Task DeleteCharacterAsync(int characterId)
        {
            var exists = await _characterRepository.CharacterExistsAsync(characterId);
            if (!exists)
                throw new InvalidOperationException("Character not found");

            await _characterRepository.DeleteCharacterAsync(characterId);
        }

        /// <summary>
        /// CharacterEntityをCharacterDataに変換する
        /// </summary>
        /// <param name="character">変換元のキャラクターエンティティ</param>
        /// <returns>変換されたキャラクターデータ</returns>
        private static CharacterData MapToCharacterData(CharacterEntity character)
        {
            return new CharacterData
            {
                CharacterId = character.CharacterId,
                PlayerUserId = character.PlayerUserId,
                Name = character.Name,
                Level = character.Level,
                PositionX = character.PositionX,
                PositionY = character.PositionY,
                CreatedAt = character.CreatedAt,
                UpdatedAt = character.UpdatedAt
            };
        }
    }
}