using MessagePack;

namespace Shared.Data
{
    [MessagePackObject]
    public class CharacterData
    {
        [Key(0)]
        public int CharacterId { get; set; }

        [Key(1)]
        public int PlayerUserId { get; set; }

        [Key(2)]
        public string Name { get; set; } = string.Empty;

        [Key(3)]
        public int Level { get; set; } = 1;

        [Key(4)]
        public float PositionX { get; set; } = 0.0f;

        [Key(5)]
        public float PositionY { get; set; } = 0.0f;

        [Key(6)]
        public DateTime CreatedAt { get; set; }

        [Key(7)]
        public DateTime UpdatedAt { get; set; }
    }
}