using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class DeleteLicenseHandler : IRequestHandler<DeleteLicenseCommand>
    {
        private readonly ILicenseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteLicenseHandler(ILicenseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLicenseCommand request, CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.LicenseId);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
