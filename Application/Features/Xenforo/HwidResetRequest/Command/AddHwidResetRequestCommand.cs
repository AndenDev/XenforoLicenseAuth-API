
using MediatR;

namespace Application.Features.Xenforo.HwidResetRequest.Command
{
    public class AddHwidResetRequestCommand : IRequest
    {
        public Domain.Entities.HwidResetRequest Request { get; }
        public AddHwidResetRequestCommand(Domain.Entities.HwidResetRequest request) => Request = request;
    }
}
