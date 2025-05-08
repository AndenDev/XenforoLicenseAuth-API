using Domain.Entities;
using MediatR;

public class GetLicensesByUserIdQuery : IRequest<IEnumerable<License>>
{
    public uint UserId { get; }

    public GetLicensesByUserIdQuery(uint userId)
    {
        UserId = userId;
    }
}
