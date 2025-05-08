
using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Command
{
    public class AddHwidBanCommand : IRequest
    {
        public string Hwid { get; }
        public string? Reason { get; }
        public string? BannedBy { get; }

        public AddHwidBanCommand(string hwid, string? reason, string? bannedBy)
            => (Hwid, Reason, BannedBy) = (hwid, reason, bannedBy);
    }
}
