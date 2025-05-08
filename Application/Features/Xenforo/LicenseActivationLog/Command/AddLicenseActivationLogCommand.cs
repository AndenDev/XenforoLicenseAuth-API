
using MediatR;

namespace Application.Features.Xenforo.LicenseActivationLog.Command
{
    public class AddLicenseActivationLogCommand : IRequest
    {
        public Domain.Entities.LicenseActivationLog Log { get; }
        public AddLicenseActivationLogCommand(Domain.Entities.LicenseActivationLog log) => Log = log;
    }

}
