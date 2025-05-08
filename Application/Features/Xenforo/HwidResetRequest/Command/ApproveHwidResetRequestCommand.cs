
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class ApproveHwidResetRequestCommand : IRequest
    {
        public uint Id { get; }
        public ApproveHwidResetRequestCommand(uint id) => Id = id;
    }
}
