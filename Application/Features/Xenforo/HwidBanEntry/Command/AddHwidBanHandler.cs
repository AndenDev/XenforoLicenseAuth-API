
using Application.Interfaces;
using Application.Interfaces.Xenforo;
using MediatR;

namespace Application.Features.Xenforo.HwidBanEntry.Command
{
    public class AddHwidBanHandler : IRequestHandler<AddHwidBanCommand>
    {
        private readonly IHwidBanListRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public AddHwidBanHandler(IHwidBanListRepository repo, IUnitOfWork unitOfWork) 
        { 
            _repo = repo; 
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddHwidBanCommand request, CancellationToken ct)
        {
            await _repo.AddBanAsync(request.Hwid, request.Reason, request.BannedBy);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
