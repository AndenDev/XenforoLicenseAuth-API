
using MediatR;

namespace Application.Features.Xenforo.Application.Queries
{
    public class GetAllApplicationsQuery : IRequest<IEnumerable<Domain.Entities.Application>>
    {
    }
}
