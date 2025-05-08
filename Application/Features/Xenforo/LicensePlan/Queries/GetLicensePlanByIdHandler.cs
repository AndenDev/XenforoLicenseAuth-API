
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.LicensePlan.Queries
{
    public class GetLicensePlanByIdHandler : IRequestHandler<GetLicensePlanByIdQuery, Domain.Entities.LicensePlan?>
    {
        private readonly ILicensePlanRepository _repo;

        public GetLicensePlanByIdHandler(ILicensePlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.Entities.LicensePlan?> Handle(GetLicensePlanByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}
