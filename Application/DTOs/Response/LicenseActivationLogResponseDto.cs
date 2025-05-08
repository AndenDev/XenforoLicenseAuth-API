using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class LicenseActivationLogResponseDto
    {
        public uint Id { get; set; }
        public uint LicenseId { get; set; }
        public uint UserId { get; set; }
        public string? Hwid { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ActivatedAt { get; set; }
        public string Event { get; set; } = null!;
    }
}
