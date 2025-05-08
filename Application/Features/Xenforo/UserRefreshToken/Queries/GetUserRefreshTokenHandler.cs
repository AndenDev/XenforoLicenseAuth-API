using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Queries
{
    public class GetUserRefreshTokenHandler : IRequestHandler<GetUserRefreshTokenQuery, Domain.Entities.UserRefreshToken?>
    {
        private readonly IUserRefreshTokenRepository _repo;
        public GetUserRefreshTokenHandler(IUserRefreshTokenRepository repo) => _repo = repo;

        public Task<Domain.Entities.UserRefreshToken?> Handle(GetUserRefreshTokenQuery request, CancellationToken ct)
            => _repo.GetByTokenAsync(request.Token);
    }
}
