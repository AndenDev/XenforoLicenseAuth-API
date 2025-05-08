
using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Command
{
    public class RemoveHwidBanCommand : IRequest
    {
        public string Hwid { get; }
        public RemoveHwidBanCommand(string hwid) => Hwid = hwid;
    }
}
