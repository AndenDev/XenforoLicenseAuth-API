using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs.Response
{
    public class LicenseResponseDto
    {
        public uint Id { get; set; }
        public uint? UserId { get; set; }
        public uint LicensePlanId { get; set; }
        public string LicenseKey { get; set; } = null!;
        public string? Hwids { get; set; }
        public LicenseStatus Status { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? SessionId { get; set; }
        public DateTime? LastHeartbeat { get; set; }
        public uint? ClientBuildId { get; set; }
        public int HwidRotationsThisMonth { get; set; }
        public DateTime LastRotationReset { get; set; }
        public bool IsPaused { get; set; }
        public DateTime? PausedAt { get; set; }
        public string? PauseReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
