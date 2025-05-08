
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.LicenseSessionLog.Command
{
    public class AddLicenseSessionLogHandler : IRequestHandler<AddLicenseSessionLogCommand>
    {
        private readonly ILicenseSessionLogRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddLicenseSessionLogHandler(ILicenseSessionLogRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork; 
        }

        public async Task<Unit> Handle(AddLicenseSessionLogCommand request, CancellationToken ct)
        {
            await _repo.AddAsync(request.Log);
            await _unitOfWork.CommitAsync();
            return Unit.Value;

        }
    }
}
