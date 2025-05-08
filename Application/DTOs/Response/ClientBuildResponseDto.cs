using Domain.Enums;

namespace Application.DTOs.Response
{
    public class ClientBuildResponseDto
    {
        public uint Id { get; set; }
        public uint ApplicationId { get; set; }
        public string Version { get; set; } = null!;
        public string BuildHash { get; set; } = null!;
        public ClientBuildStatus Status { get; set; }
        public DateTime ReleasedAt { get; set; }
        public string? Notes { get; set; }
    }
}
