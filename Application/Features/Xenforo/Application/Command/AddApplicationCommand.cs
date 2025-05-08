using MediatR;

namespace Application.Features.Xenforo.Application.Command
{
    public class AddApplicationCommand : IRequest
    {
        public string Name { get; }
        public string Status { get; }
        public string? LogoUrl { get; }
        public string? Description { get; }
        public DateTime CreatedAt { get; }
        public bool IsActive { get; }

        public AddApplicationCommand(string name, string status, string? logoUrl, string? description, DateTime createdAt, bool isActive)
            => (Name, Status, LogoUrl, Description, CreatedAt, IsActive) = (name, status, logoUrl, description, createdAt, isActive);
    }
}
