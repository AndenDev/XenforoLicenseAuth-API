using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.Game.Queries
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, Domain.Entities.Game?>
    {
        private readonly IGameRepository _repository;

        public GetGameByIdHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.Game?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
