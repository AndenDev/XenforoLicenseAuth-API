using Domain.Entities;
using MediatR;

namespace Application.Features.Xenforo.Game.Queries
{
    public record GetAllGamesQuery() : IRequest<IEnumerable<Domain.Entities.Game>>;
}
