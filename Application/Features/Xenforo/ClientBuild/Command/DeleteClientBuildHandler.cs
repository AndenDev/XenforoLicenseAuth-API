using Application.Interfaces.Xenforo;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class DeleteClientBuildHandler : IRequestHandler<DeleteClientBuildCommand>
    {
        private readonly IClientBuildRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientBuildHandler(IClientBuildRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteClientBuildCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
