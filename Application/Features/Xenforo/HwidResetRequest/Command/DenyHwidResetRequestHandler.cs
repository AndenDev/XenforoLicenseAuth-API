
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class DenyHwidResetRequestHandler : IRequestHandler<DenyHwidResetRequestCommand>
    {
        private readonly IHwidResetRequestRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public DenyHwidResetRequestHandler(IHwidResetRequestRepository repo, IUnitOfWork unitOfWork) 
        { 
            _repo = repo; 
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DenyHwidResetRequestCommand request, CancellationToken ct)
        {
            await _repo.DenyAsync(request.Id);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
