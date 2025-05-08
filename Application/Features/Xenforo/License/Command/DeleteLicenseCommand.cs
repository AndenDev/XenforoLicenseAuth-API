using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class DeleteLicenseCommand : IRequest
    {
        public uint LicenseId { get; }

        public DeleteLicenseCommand(uint licenseId)
        {
            LicenseId = licenseId;
        }
    }
}
