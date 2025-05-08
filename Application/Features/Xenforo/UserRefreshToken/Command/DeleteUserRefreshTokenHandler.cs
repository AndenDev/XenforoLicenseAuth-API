
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Command
{
    public class DeleteUserRefreshTokenHandler : IRequestHandler<DeleteUserRefreshTokenCommand>
    {
        private readonly IUserRefreshTokenRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserRefreshTokenHandler(IUserRefreshTokenRepository repo, IUnitOfWork unitOfWork) 
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUserRefreshTokenCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Token);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
