using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServer.Entities
{
    public enum UserRole
    {
        Player = 0,
        Admin = 1
    }

    public enum AccountStatus
    {
        Active = 0,
        Suspended = 1,
        Deleted = 2
    }

    public class UserRoleEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.Player;

        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public int? SuspendedByUserId { get; set; }

        public DateTime? SuspendedAt { get; set; }

        public string? SuspensionReason { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual PlayerEntity? Player { get; set; }

        [ForeignKey("SuspendedByUserId")]
        public virtual PlayerEntity? SuspendedByUser { get; set; }
    }
}