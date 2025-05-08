using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class UserRefreshTokenResponseDto
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
