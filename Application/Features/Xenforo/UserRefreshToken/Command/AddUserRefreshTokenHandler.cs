
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.UserRefreshToken.Command
{
    public class AddUserRefreshTokenHandler : IRequestHandler<AddUserRefreshTokenCommand>
    {
        private readonly IUserRefreshTokenRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddUserRefreshTokenHandler(IUserRefreshTokenRepository repo, IUnitOfWork unitOfWork)
        { 
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddUserRefreshTokenCommand request, CancellationToken ct)
        {
            await _repo.AddAsync(request.Token);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
