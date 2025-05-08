
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.LicenseActivationLog.Command
{
    public class AddLicenseActivationLogHandler : IRequestHandler<AddLicenseActivationLogCommand>
    {
        private readonly ILicenseActivationLogRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddLicenseActivationLogHandler(ILicenseActivationLogRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddLicenseActivationLogCommand request, CancellationToken ct)
        {
            await _repo.AddAsync(request.Log);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
