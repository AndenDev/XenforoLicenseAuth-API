using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Query
{
    public class GetClientBuildsByApplicationIdHandler : IRequestHandler<GetClientBuildsByApplicationIdQuery, IEnumerable<Domain.Entities.ClientBuild>>
    {
        private readonly IClientBuildRepository _repo;

        public GetClientBuildsByApplicationIdHandler(IClientBuildRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.ClientBuild>> Handle(GetClientBuildsByApplicationIdQuery request, CancellationToken ct)
        {
            return await _repo.GetByApplicationIdAsync(request.ApplicationId);
        }
    }
}
