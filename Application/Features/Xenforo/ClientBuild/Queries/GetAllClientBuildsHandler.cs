using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Query
{
    public class GetAllClientBuildsHandler : IRequestHandler<GetAllClientBuildsQuery, IEnumerable<Domain.Entities.ClientBuild>>
    {
        private readonly IClientBuildRepository _repo;

        public GetAllClientBuildsHandler(IClientBuildRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Domain.Entities.ClientBuild>> Handle(GetAllClientBuildsQuery request, CancellationToken ct)
        {
            return await _repo.GetAllAsync();
        }
    }
}
