using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class UpdateLicenseCommand : IRequest
    {
        public Domain.Entities.License License { get; }

        public UpdateLicenseCommand(Domain.Entities.License license)
        {
            License = license;
        }
    }
}
