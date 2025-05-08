
using Domain.Enums;

namespace Application.DTOs.Response
{
    public class ApplicationResponseDto
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public ApplicationStatus Status { get; set; }
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
