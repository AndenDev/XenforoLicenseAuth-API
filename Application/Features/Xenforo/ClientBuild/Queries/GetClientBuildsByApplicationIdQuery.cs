using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.ClientBuild.Query
{
    public class GetClientBuildsByApplicationIdQuery : IRequest<IEnumerable<Domain.Entities.ClientBuild>>
    {
        public uint ApplicationId { get; }

        public GetClientBuildsByApplicationIdQuery(uint applicationId) => ApplicationId = applicationId;
    }
}
