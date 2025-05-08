using Application.Interfaces.Xenforo;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.Game.Command
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameCommand>
    {
        private readonly IGameRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGameHandler(IGameRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken ct)
        {
            var game = new Domain.Entities.Game
            {
                Id = request.Id,
                Name = request.Name,
                Status = Enum.Parse<GameStatus>(request.Status),
                LogoUrl = request.LogoUrl,
                LastUpdated = request.LastUpdated,
                IsActive = request.IsActive
            };

            await _repo.UpdateAsync(game);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
