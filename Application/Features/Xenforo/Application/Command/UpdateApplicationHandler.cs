using Application.Interfaces.Xenforo;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.Application.Command
{
    public class UpdateApplicationHandler : IRequestHandler<UpdateApplicationCommand>
    {
        private readonly IApplicationRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApplicationHandler(IApplicationRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateApplicationCommand request, CancellationToken ct)
        {
            var app = new Domain.Entities.Application
            {
                Id = request.Id,
                Name = request.Name,
                Status = Enum.Parse<ApplicationStatus>(request.Status),
                LogoUrl = request.LogoUrl,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await _repo.UpdateAsync(app);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
