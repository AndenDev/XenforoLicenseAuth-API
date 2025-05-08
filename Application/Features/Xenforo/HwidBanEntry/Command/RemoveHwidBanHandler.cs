
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Command
{
    public class RemoveHwidBanHandler : IRequestHandler<RemoveHwidBanCommand>
    {
        private readonly IHwidBanListRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public RemoveHwidBanHandler(IHwidBanListRepository repo, IUnitOfWork unitOfWork) 
        { 
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveHwidBanCommand request, CancellationToken ct)
        {
            await _repo.RemoveBanAsync(request.Hwid);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
