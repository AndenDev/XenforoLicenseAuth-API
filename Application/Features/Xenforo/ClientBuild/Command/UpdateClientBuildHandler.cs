using Application.Interfaces.Xenforo;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.ClientBuild.Command
{
    public class UpdateClientBuildHandler : IRequestHandler<UpdateClientBuildCommand>
    {
        private readonly IClientBuildRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClientBuildHandler(IClientBuildRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateClientBuildCommand request, CancellationToken ct)
        {
            var build = new Domain.Entities.ClientBuild
            {
                Id = request.Id,
                ApplicationId = request.ApplicationId,
                Version = request.Version,
                BuildHash = request.BuildHash,
                Status = Enum.Parse<ClientBuildStatus>(request.Status),
                ReleasedAt = request.ReleasedAt,
                Notes = request.Notes
            };

            await _repo.UpdateAsync(build);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
