using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Queries
{
    public class IsHwidBannedQuery : IRequest<bool>
    {
        public string Hwid { get; }
        public IsHwidBannedQuery(string hwid) => Hwid = hwid;
    }
}
