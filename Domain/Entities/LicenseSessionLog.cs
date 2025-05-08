using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LicenseSessionLog
    {
        public uint Id { get; set; }
        public uint LicenseId { get; set; }
        public string SessionId { get; set; } = null!;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }
        public string? EndedReason { get; set; }
        public string? IpAddress { get; set; }
        public string? Hwid { get; set; }
    }
}
