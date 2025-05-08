using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Xenforo.LicenseActivationLog.Query
{
    public class GetLicenseActivationLogsByLicenseIdHandler : IRequestHandler<GetLicenseActivationLogsByLicenseIdQuery, IEnumerable<Domain.Entities.LicenseActivationLog>>
    {
        private readonly ILicenseActivationLogRepository _repo;

        public GetLicenseActivationLogsByLicenseIdHandler(ILicenseActivationLogRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.LicenseActivationLog>> Handle(GetLicenseActivationLogsByLicenseIdQuery request, CancellationToken ct)
        {
            return await _repo.GetByLicenseIdAsync(request.LicenseId);
        }
    }
}
