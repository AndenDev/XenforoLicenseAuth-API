using Application.Interfaces.Xenforo;
using Application.Interfaces;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class AddClientBuildHandler : IRequestHandler<AddClientBuildCommand>
    {
        private readonly IClientBuildRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public AddClientBuildHandler(IClientBuildRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddClientBuildCommand request, CancellationToken ct)
        {
            var build = new Domain.Entities.ClientBuild
            {
                ApplicationId = request.ApplicationId,
                Version = request.Version,
                BuildHash = request.BuildHash,
                Status = Enum.Parse<ClientBuildStatus>(request.Status),
                ReleasedAt = request.ReleasedAt,
                Notes = request.Notes
            };

            await _repo.AddAsync(build);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
