
using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Query
{
    public class GetAllClientBuildsQuery : IRequest<IEnumerable<Domain.Entities.ClientBuild>>
    {
        public GetAllClientBuildsQuery() { }
    }
}
