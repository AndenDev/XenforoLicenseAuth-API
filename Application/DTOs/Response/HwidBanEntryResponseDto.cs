using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class HwidBanEntryResponseDto
    {
        public uint Id { get; set; }
        public string Hwid { get; set; } = null!;
        public string? Reason { get; set; }
        public DateTime BannedAt { get; set; }
        public string? BannedByUsername { get; set; }
        public bool IsActive { get; set; }
    }
}
