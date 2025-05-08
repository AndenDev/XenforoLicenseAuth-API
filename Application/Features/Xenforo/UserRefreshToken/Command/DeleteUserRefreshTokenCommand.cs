using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Command
{
    public class DeleteUserRefreshTokenCommand : IRequest
    {
        public string Token { get; }
        public DeleteUserRefreshTokenCommand(string token) => Token = token;
    }

}
