
using Application.Features.Xenforo.License.Queries;
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetLicenseByKeyHandler : IRequestHandler<GetLicenseByKeyQuery, License?>
{
    private readonly ILicenseRepository _licenseRepository;

    public GetLicenseByKeyHandler(ILicenseRepository licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    public async Task<License?> Handle(GetLicenseByKeyQuery request, CancellationToken cancellationToken)
    {
        return await _licenseRepository.GetByKeyAsync(request.LicenseKey);
    }
}
