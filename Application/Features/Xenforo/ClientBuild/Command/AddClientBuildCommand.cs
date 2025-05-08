using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class AddClientBuildCommand : IRequest
    {
        public uint ApplicationId { get; }
        public string Version { get; }
        public string BuildHash { get; }
        public string Status { get; }
        public DateTime ReleasedAt { get; }
        public string? Notes { get; }

        public AddClientBuildCommand(uint applicationId, string version, string buildHash, string status, DateTime releasedAt, string? notes) =>
            (ApplicationId, Version, BuildHash, Status, ReleasedAt, Notes) = (applicationId, version, buildHash, status, releasedAt, notes);
    }
}
