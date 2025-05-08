
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class UpdateLicenseHandler : IRequestHandler<UpdateLicenseCommand>
    {
        private readonly ILicenseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateLicenseHandler(ILicenseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
        {
            await _repo.UpdateAsync(request.License);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }

}
