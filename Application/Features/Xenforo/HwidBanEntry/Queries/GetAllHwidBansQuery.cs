using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Xenforo.HwidBan.Query
{
    public class GetAllHwidBansQuery : IRequest<IEnumerable<Domain.Entities.HwidBanEntry>>
    {
        public GetAllHwidBansQuery() { }
    }
}
