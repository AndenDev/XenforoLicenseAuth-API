using MediatR;

namespace Application.Features.Xenforo.Game.Queries
{
    public record GetGameByIdQuery(uint Id) : IRequest<Domain.Entities.Game?>;
}
