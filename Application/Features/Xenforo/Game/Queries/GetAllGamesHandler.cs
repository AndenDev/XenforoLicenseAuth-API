
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.Game.Queries
{
    public class GetAllGamesHandler : IRequestHandler<GetAllGamesQuery, IEnumerable<Domain.Entities.Game>>
    {
        private readonly IGameRepository _repository;

        public GetAllGamesHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Domain.Entities.Game>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
