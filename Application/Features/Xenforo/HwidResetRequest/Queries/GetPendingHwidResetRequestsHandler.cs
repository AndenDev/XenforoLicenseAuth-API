using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Queries
{
    public class GetPendingHwidResetRequestsHandler : IRequestHandler<GetPendingHwidResetRequestsQuery, IEnumerable<Domain.Entities.HwidResetRequest>>
    {
        private readonly IHwidResetRequestRepository _repo;
        public GetPendingHwidResetRequestsHandler(IHwidResetRequestRepository repo) => _repo = repo;

        public Task<IEnumerable<Domain.Entities.HwidResetRequest>> Handle(GetPendingHwidResetRequestsQuery request, CancellationToken ct)
            => _repo.GetPendingAsync();
    }
}
