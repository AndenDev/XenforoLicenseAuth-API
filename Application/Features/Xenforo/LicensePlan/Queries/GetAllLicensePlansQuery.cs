using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.LicensePlan.Queries
{
    public class GetAllLicensePlansHandler : IRequestHandler<GetAllLicensePlansQuery, IEnumerable<Domain.Entities.LicensePlan>>
    {
        private readonly ILicensePlanRepository _repo;

        public GetAllLicensePlansHandler(ILicensePlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.LicensePlan>> Handle(GetAllLicensePlansQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
