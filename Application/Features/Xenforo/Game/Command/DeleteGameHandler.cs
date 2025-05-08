using Application.Interfaces.Xenforo;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Xenforo.Game.Command
{
    public class DeleteGameHandler : IRequestHandler<DeleteGameCommand>
    {
        private readonly IGameRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGameHandler(IGameRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteGameCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
