using Application.Interfaces.Xenforo;
using Application.Interfaces;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.Game.Command
{
    public class AddGameHandler : IRequestHandler<AddGameCommand>
    {
        private readonly IGameRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public AddGameHandler(IGameRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddGameCommand request, CancellationToken ct)
        {
            var game = new Domain.Entities.Game
            {
                Name = request.Name,
                Status = Enum.Parse<GameStatus>(request.Status),
                LogoUrl = request.LogoUrl,
                LastUpdated = request.LastUpdated,
                IsActive = request.IsActive
            };

            await _repo.Addsync(game);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
