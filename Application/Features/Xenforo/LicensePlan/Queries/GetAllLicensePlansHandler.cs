
using MediatR;

namespace Application.Features.Xenforo.LicensePlan.Queries
{
    public class GetAllLicensePlansQuery : IRequest<IEnumerable<Domain.Entities.LicensePlan>>
    {
    }
}
