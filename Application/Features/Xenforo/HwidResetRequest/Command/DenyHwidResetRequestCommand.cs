
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class DenyHwidResetRequestCommand : IRequest
    {
        public uint Id { get; }
        public DenyHwidResetRequestCommand(uint id) => Id = id;
    }
}
