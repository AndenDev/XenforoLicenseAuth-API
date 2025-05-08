using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.LicenseActivationLog.Query
{
    public class GetLicenseActivationLogsByUserIdQuery : IRequest<IEnumerable<Domain.Entities.LicenseActivationLog>>
    {
        public uint UserId { get; }

        public GetLicenseActivationLogsByUserIdQuery(uint userId) => UserId = userId;
    }
}
