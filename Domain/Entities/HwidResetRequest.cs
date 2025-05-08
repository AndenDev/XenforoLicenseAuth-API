using Domain.Enums;

namespace Domain.Entities
{
    public class HwidResetRequest
    {
        public uint Id { get; set; }
        public uint LicenseId { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAt { get; set; }
        public HwidResetRequestStatus Status { get; set; } = HwidResetRequestStatus.Pending;
        public string? Reason { get; set; }
    }
}
