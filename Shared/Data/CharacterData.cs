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
    }
}