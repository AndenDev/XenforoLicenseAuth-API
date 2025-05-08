using MediatR;

namespace Application.Features.Xenforo.License.Queries
{
    public class GetLicenseByKeyQuery : IRequest<Domain.Entities.License?>
    {
        public string LicenseKey { get; }

        public GetLicenseByKeyQuery(string licenseKey)
        {
            LicenseKey = licenseKey;
        }
    }

}
