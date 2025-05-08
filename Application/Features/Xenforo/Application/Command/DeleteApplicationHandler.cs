using Application.Interfaces.Xenforo;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Xenforo.Application.Command
{
    public class DeleteApplicationHandler : IRequestHandler<DeleteApplicationCommand>
    {
        private readonly IApplicationRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApplicationHandler(IApplicationRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteApplicationCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
