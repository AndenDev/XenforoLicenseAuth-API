using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class LicensePlanResponseDto
    {
        public uint Id { get; set; }
        public uint ApplicationId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
