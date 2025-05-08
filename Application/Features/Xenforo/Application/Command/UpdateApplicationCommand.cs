using MediatR;

namespace Application.Features.Xenforo.Application.Command
{
    public class UpdateApplicationCommand : IRequest
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public UpdateApplicationCommand(uint id, string name, string status, string? logoUrl, string? description, bool isActive) =>
            (Id, Name, Status, LogoUrl, Description, IsActive) = (id, name, status, logoUrl, description, isActive);
    }
}
