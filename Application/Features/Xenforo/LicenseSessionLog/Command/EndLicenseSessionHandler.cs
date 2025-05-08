
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.LicenseSessionLog.Command
{
    public class EndLicenseSessionHandler : IRequestHandler<EndLicenseSessionCommand>
    {
        private readonly ILicenseSessionLogRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public EndLicenseSessionHandler(ILicenseSessionLogRepository repo, IUnitOfWork unitOfWorkd)
        {
            _repo = repo;
            _unitOfWork = unitOfWorkd;
        }

        public async Task<Unit> Handle(EndLicenseSessionCommand request, CancellationToken ct)
        {
            await _repo.EndSessionAsync(request.SessionId, request.EndedReason);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
