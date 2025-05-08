using Domain.Enums;

namespace Domain.Entities
{
    public class Game
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public GameStatus Status { get; set; } = GameStatus.Working;
        public string? LogoUrl { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
