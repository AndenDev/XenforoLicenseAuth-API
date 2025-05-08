
using Application.Interfaces.Xenforo;
using Domain.Entities;
using MediatR;

public class GetLicensesByUserIdHandler : IRequestHandler<GetLicensesByUserIdQuery, IEnumerable<License>>
{
    private readonly ILicenseRepository _repo;

    public GetLicensesByUserIdHandler(ILicenseRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<License>> Handle(GetLicensesByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByUserIdAsync(request.UserId);
    }
}
