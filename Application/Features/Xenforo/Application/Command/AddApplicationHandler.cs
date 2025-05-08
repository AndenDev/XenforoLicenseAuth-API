using Application.Interfaces.Xenforo;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Enums;

namespace Application.Features.Xenforo.Application.Command
{
    public class AddApplicationHandler : IRequestHandler<AddApplicationCommand>
    {
        private readonly IApplicationRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public AddApplicationHandler(IApplicationRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddApplicationCommand request, CancellationToken ct)
        {
            var app = new Domain.Entities.Application
            {
                Name = request.Name,
                Status = Enum.Parse<ApplicationStatus>(request.Status),
                LogoUrl = request.LogoUrl,
                Description = request.Description,
                CreatedAt = request.CreatedAt,
                IsActive = request.IsActive
            };

            await _repo.AddAsync(app);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
