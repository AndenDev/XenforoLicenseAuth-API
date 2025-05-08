
using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Command
{
    public class AddUserRefreshTokenCommand : IRequest
    {
        public Domain.Entities.UserRefreshToken Token { get; }
        public AddUserRefreshTokenCommand(Domain.Entities.UserRefreshToken token) => Token = token;
    }
}
