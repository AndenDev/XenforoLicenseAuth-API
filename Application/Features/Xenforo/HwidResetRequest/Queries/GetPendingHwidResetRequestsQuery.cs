using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Queries
{
    public class GetPendingHwidResetRequestsQuery : IRequest<IEnumerable<Domain.Entities.HwidResetRequest>> { }
}
