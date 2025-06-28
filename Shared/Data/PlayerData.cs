using MessagePack;
namespace Shared.Data
{
    [MessagePackObject]
    public class PlayerData
    {
        [Key(0)]
        public int UserId { get; set; }
        [Key(1)]
        public string UserName { get; set; } = string.Empty;
        [Key(2)]
        public int Money { get; set; }
    }
}