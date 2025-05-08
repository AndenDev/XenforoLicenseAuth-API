using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.LicenseSessionLog.Query
{
    public class GetActiveLicenseSessionsQuery : IRequest<IEnumerable<Domain.Entities.LicenseSessionLog>>
    {
        public GetActiveLicenseSessionsQuery() { }
    }
}
