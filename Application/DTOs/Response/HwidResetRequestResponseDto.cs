
using Domain.Enums;

namespace Application.DTOs.Response
{
    public class HwidResetRequestResponseDto
    {
        public uint Id { get; set; }
        public uint LicenseId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public HwidResetRequestStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
