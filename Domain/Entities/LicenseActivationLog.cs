using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LicenseActivationLog
    {
        public uint Id { get; set; }
        public uint LicenseId { get; set; }
        public uint UserId { get; set; }
        public string? Hwid { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ActivatedAt { get; set; } = DateTime.UtcNow;
        public string Event { get; set; } = null!; // activate, hwid_change, etc.
    }
}
