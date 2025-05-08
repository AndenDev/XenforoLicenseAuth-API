
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.Application.Queries
{
    public class GetAllApplicationsHandler : IRequestHandler<GetAllApplicationsQuery, IEnumerable<Domain.Entities.Application>>
    {
        private readonly IApplicationRepository _repo;

        public GetAllApplicationsHandler(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.Application>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }

}
