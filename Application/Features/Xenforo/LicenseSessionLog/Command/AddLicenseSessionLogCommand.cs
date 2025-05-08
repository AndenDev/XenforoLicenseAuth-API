
using MediatR;

namespace Application.Features.Xenforo.LicenseSessionLog.Command
{
    public class AddLicenseSessionLogCommand : IRequest
    {
        public Domain.Entities.LicenseSessionLog Log { get; }
        public AddLicenseSessionLogCommand(Domain.Entities.LicenseSessionLog log) => Log = log;
    }

}
