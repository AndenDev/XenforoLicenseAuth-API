using Domain.Enums;

namespace Domain.Entities
{
    public class Application
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Working;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
