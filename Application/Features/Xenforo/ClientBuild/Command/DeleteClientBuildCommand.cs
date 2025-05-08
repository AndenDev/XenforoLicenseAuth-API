using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class DeleteClientBuildCommand : IRequest
    {
        public uint Id { get; }

        public DeleteClientBuildCommand(uint id) => Id = id;
    }
}
