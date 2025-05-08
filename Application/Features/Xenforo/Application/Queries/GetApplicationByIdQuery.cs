using MediatR;

namespace Application.Features.Xenforo.Application.Queries
{
    public class GetApplicationByIdQuery : IRequest<Domain.Entities.Application?>
    {
        public uint Id { get; }
        public GetApplicationByIdQuery(uint id) => Id = id;
    }

}
