using MediatR;

namespace Application.Features.Xenforo.Game.Command
{
    public class UpdateGameCommand : IRequest
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string? LogoUrl { get; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }

        public UpdateGameCommand(uint id, string name, string status, string? logoUrl, DateTime lastUpdated, bool isActive) =>
            (Id, Name, Status, LogoUrl, LastUpdated, IsActive) = (id, name, status, logoUrl, lastUpdated, isActive);
    }
}
