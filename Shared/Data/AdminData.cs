using MessagePack;

namespace Shared.Data
{
    [MessagePackObject]
    public class AdminData
    {
        [Key(0)]
        public int UserId { get; set; }

        [Key(1)]
        public int Role { get; set; }

        [Key(2)]
        public int Status { get; set; }

        [Key(3)]
        public int? SuspendedByUserId { get; set; }

        [Key(4)]
        public DateTime? SuspendedAt { get; set; }

        [Key(5)]
        public string? SuspensionReason { get; set; }
    }

    [MessagePackObject]
    public class SuspensionRequest
    {
        [Key(0)]
        public int TargetUserId { get; set; }

        [Key(1)]
        public int AdminUserId { get; set; }

        [Key(2)]
        public string Reason { get; set; } = string.Empty;
    }
}