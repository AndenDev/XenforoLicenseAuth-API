
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.License.Command
{
    public class AddLicenseHandler : IRequestHandler<AddLicenseCommand>
    {
        private readonly ILicenseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddLicenseHandler(ILicenseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddLicenseCommand request, CancellationToken cancellationToken)
        {
            await _repo.AddAsync(request.License);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
