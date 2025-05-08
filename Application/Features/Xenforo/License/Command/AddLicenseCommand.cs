using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class AddLicenseCommand : IRequest
    {
        public Domain.Entities.License License { get; }

        public AddLicenseCommand(Domain.Entities.License license)
        {
            License = license;
        }
    }

}
