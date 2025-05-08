
using Domain.Enums;

namespace Application.DTOs.Response
{
    public class GameResponseDto
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public GameStatus Status { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
