using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.LicenseActivationLog.Query
{
    public class GetLicenseActivationLogsByLicenseIdQuery : IRequest<IEnumerable<Domain.Entities.LicenseActivationLog>>
    {
        public uint LicenseId { get; }

        public GetLicenseActivationLogsByLicenseIdQuery(uint licenseId) => LicenseId = licenseId;
    }
}
