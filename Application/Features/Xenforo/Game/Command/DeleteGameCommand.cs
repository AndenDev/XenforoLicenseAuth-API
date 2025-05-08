using MediatR;

namespace Application.Features.Xenforo.Game.Command
{
    public class DeleteGameCommand : IRequest
    {
        public uint Id { get; }

        public DeleteGameCommand(uint id) => Id = id;
    }
}
