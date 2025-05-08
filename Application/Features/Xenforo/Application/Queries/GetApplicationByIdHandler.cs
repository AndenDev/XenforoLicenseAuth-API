using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.Application.Queries
{
    public class GetApplicationByIdHandler : IRequestHandler<GetApplicationByIdQuery, Domain.Entities.Application?>
    {
        private readonly IApplicationRepository _repo;

        public GetApplicationByIdHandler(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.Entities.Application?> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }

}
