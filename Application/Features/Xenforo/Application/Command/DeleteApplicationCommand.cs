using MediatR;

namespace Application.Features.Xenforo.Application.Command
{
    public class DeleteApplicationCommand : IRequest
    {
        public uint Id { get; }

        public DeleteApplicationCommand(uint id) => Id = id;
    }
}
