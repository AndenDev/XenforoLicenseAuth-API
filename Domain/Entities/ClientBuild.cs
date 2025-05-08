using Domain.Enums;

namespace Domain.Entities
{
    public class ClientBuild
    {
        public uint Id { get; set; }
        public uint ApplicationId { get; set; }
        public string Version { get; set; } = null!;
        public string BuildHash { get; set; } = null!;
        public ClientBuildStatus Status { get; set; } = ClientBuildStatus.Approved;
        public DateTime ReleasedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}
