
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class AddHwidResetRequestHandler : IRequestHandler<AddHwidResetRequestCommand>
    {
        private readonly IHwidResetRequestRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddHwidResetRequestHandler(IHwidResetRequestRepository repo, IUnitOfWork unitOfWork) 
        { 
            _repo = repo; 
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddHwidResetRequestCommand request, CancellationToken ct)
        {
            await _repo.AddRequestAsync(request.Request);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
