
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class ApproveHwidResetRequestHandler : IRequestHandler<ApproveHwidResetRequestCommand>
    {
        private readonly IHwidResetRequestRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public ApproveHwidResetRequestHandler(IHwidResetRequestRepository repo, IUnitOfWork unitOfWork) 
        { 
            _repo = repo; 
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ApproveHwidResetRequestCommand request, CancellationToken ct)
        {
            await _repo.ApproveAsync(request.Id);
            return Unit.Value;
        }
    }
}
