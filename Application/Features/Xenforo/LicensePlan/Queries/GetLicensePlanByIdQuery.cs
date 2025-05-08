using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Xenforo.LicensePlan.Queries
{
    public class GetLicensePlanByIdQuery : IRequest<Domain.Entities.LicensePlan?>
    {
        public uint Id { get; }

        public GetLicensePlanByIdQuery(uint id)
        {
            Id = id;
        }
    }
}
