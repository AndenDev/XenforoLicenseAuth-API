using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class UpdateClientBuildCommand : IRequest
    {
        public uint Id { get; }
        public uint ApplicationId { get; }
        public string Version { get; }
        public string BuildHash { get; }
        public string Status { get; }
        public DateTime ReleasedAt { get; }
        public string? Notes { get; }

        public UpdateClientBuildCommand(uint id, uint applicationId, string version, string buildHash, string status, DateTime releasedAt, string? notes) =>
            (Id, ApplicationId, Version, BuildHash, Status, ReleasedAt, Notes) = (id, applicationId, version, buildHash, status, releasedAt, notes);
    }
}
