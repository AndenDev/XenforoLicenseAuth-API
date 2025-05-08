using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Xenforo.LicenseSessionLog.Query
{
    public class GetLicenseSessionLogsByLicenseIdHandler : IRequestHandler<GetLicenseSessionLogsByLicenseIdQuery, IEnumerable<Domain.Entities.LicenseSessionLog>>
    {
        private readonly ILicenseSessionLogRepository _repo;

        public GetLicenseSessionLogsByLicenseIdHandler(ILicenseSessionLogRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.LicenseSessionLog>> Handle(GetLicenseSessionLogsByLicenseIdQuery request, CancellationToken ct)
        {
            return await _repo.GetByLicenseIdAsync(request.LicenseId);
        }
    }
}
