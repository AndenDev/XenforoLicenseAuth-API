using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Xenforo.License.Query
{
    public class GetAllLicensesHandler : IRequestHandler<GetAllLicensesQuery, IEnumerable<Domain.Entities.License>>
    {
        private readonly ILicenseRepository _repo;

        public GetAllLicensesHandler(ILicenseRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.License>> Handle(GetAllLicensesQuery request, CancellationToken ct)
        {
            return await _repo.GetAllAsync();
        }
    }
}
