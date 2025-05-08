
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Queries
{
    public class IsHwidBannedHandler : IRequestHandler<IsHwidBannedQuery, bool>
    {
        private readonly IHwidBanListRepository _repo;
        public IsHwidBannedHandler(IHwidBanListRepository repo) => _repo = repo;

        public Task<bool> Handle(IsHwidBannedQuery request, CancellationToken ct) => _repo.IsBannedAsync(request.Hwid);
    }
}
