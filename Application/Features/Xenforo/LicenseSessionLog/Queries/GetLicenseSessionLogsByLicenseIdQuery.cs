using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.LicenseSessionLog.Query
{
    public class GetLicenseSessionLogsByLicenseIdQuery : IRequest<IEnumerable<Domain.Entities.LicenseSessionLog>>
    {
        public uint LicenseId { get; }

        public GetLicenseSessionLogsByLicenseIdQuery(uint licenseId) => LicenseId = licenseId;
    }
}
