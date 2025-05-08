using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Xenforo.HwidBan.Query
{
    public class GetAllHwidBansHandler : IRequestHandler<GetAllHwidBansQuery, IEnumerable<Domain.Entities.HwidBanEntry>>
    {
        private readonly IHwidBanListRepository _repo;

        public GetAllHwidBansHandler(IHwidBanListRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.HwidBanEntry>> Handle(GetAllHwidBansQuery request, CancellationToken ct)
        {
            return await _repo.GetAllBansAsync();
        }
    }
}
