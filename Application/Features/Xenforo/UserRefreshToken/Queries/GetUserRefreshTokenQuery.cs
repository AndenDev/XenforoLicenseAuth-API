using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Queries
{
    public class GetUserRefreshTokenQuery : IRequest<Domain.Entities.UserRefreshToken?>
    {
        public string Token { get; }
        public GetUserRefreshTokenQuery(string token) => Token = token;
    }
}
