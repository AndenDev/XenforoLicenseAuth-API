using MediatR;

namespace Application.Features.Xenforo.Game.Command
{
    public class AddGameCommand : IRequest
    {
        public string Name { get; }
        public string Status { get; }
        public string? LogoUrl { get; }
        public DateTime LastUpdated { get; }
        public bool IsActive { get; }

        public AddGameCommand(string name, string status, string? logoUrl, DateTime lastUpdated, bool isActive) =>
            (Name, Status, LogoUrl, LastUpdated, IsActive) = (name, status, logoUrl, lastUpdated, isActive);
    }
}
