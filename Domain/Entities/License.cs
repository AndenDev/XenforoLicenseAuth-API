using Domain.Enums;

namespace Domain.Entities
{
    public class License
    {
        public uint Id { get; set; }
        public uint? UserId { get; set; }
        public uint LicensePlanId { get; set; }
        public string LicenseKey { get; set; } = null!;
        public string? Hwids { get; set; }
        public LicenseStatus Status { get; set; } = LicenseStatus.Unactivated;
        public DateTime? ExpiresAt { get; set; }
        public string? SessionId { get; set; }
        public DateTime? LastHeartbeat { get; set; }
        public uint? ClientBuildId { get; set; }
        public int HwidRotationsThisMonth { get; set; }
        public DateTime LastRotationReset { get; set; } = DateTime.UtcNow;
        public bool IsPaused { get; set; }
        public DateTime? PausedAt { get; set; }
        public string? PauseReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
