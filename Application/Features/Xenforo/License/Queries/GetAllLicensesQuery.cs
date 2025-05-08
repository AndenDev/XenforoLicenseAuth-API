using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.License.Query
{
    public class GetAllLicensesQuery : IRequest<IEnumerable<Domain.Entities.License>>
    {
        public GetAllLicensesQuery() { }
    }
}
